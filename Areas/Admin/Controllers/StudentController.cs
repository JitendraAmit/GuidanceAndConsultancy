using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GuidanceConsultancy.Areas.Admin.Models.StudentViewModel;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;
using GuidanceConsultancy.Models.db;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
  
    public class StudentController : BaseController
    {
        public readonly DishaGuidanceEntities _context;
        public StudentController()
        {
            _context = new DishaGuidanceEntities();
        }
        // GET: Admin/Student

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        public ActionResult Index()
        {
            StudentListViewModel viewModel = new StudentListViewModel();
            if(User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.SuperAdministrator.RoleName) ||User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.Administrator.RoleName))
                {
                    var getData = _context.Mst_Student.Where(m => m.IsDelete == false).OrderByDescending(m => m.Id).ToList();
                    viewModel.StudentModel = getData;
                }
                else if (User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.User.RoleName))
                {
                    var getDataByUser = _context.Mst_Student.Where(m => m.IsDelete == false && m.CreatedBy==User.UserId).OrderByDescending(m => m.Id).ToList();
                    viewModel.StudentModel = getDataByUser;
                }
                else
                {

                }
            }

                   
           
            return View(viewModel);
        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        [HttpGet]
        public ActionResult Create()
        {
            CreateEditStudentViewModel viewModel = new CreateEditStudentViewModel();
            var getSubTestList = _context.Mst_SubTest.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            viewModel.SelectClassList = GetClassSelectList();
            viewModel.SelectSchoolList = GetSchoolSelectList();
            viewModel.SelectGenderList = GetGenderSelectList();
            viewModel.SubTestModel = getSubTestList;
            return View(viewModel);
            
        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        [HttpPost]
        public JsonResult Create(CreateEditStudentViewModel studentData, ObtainMarks [] subTestData)
        {
            try
            {
                Mst_Student obj = new Mst_Student();
                obj.Name = studentData.Name;
                obj.FatherName = studentData.FatherName;
                obj.EmailId = studentData.EmailId;
                obj.ContactNo = studentData.ContactNo;
                obj.ClassId = studentData.ClassId;
                obj.SchoolId = studentData.SchoolId;
                //obj.SchoolTypeId = studentData.SchoolTypeId;
                obj.GenderId = studentData.GenderId;
                obj.IsActive = true;
                obj.IsDelete = false;
                obj.IsVerified = false;
                obj.CreatedOn = Helpers.Utilities.GetCurrentDateTime();
                obj.CreatedBy = User.UserId;
                _context.Mst_Student.Add(obj);
                _context.SaveChanges();

                var getLastRecordId = _context.Mst_Student.Max(m => m.Id);
                List<ChartFields> _chart = new List<ChartFields>();
                //List<int> graphStenScoreList = new List<int>();
                //List<string> graphSubTestList = new List<string>();
                if(subTestData==null)
                {

                }
                else
                {
                    var _schoolTypeId = _context.Mst_School.FirstOrDefault(m => m.Id == studentData.SchoolId).Mst_SchoolType.Id;
                    foreach (var items in subTestData)
                    {
                        var getStenScoreData = _context.Mst_Norms.FirstOrDefault(m => m.ClassId == studentData.ClassId && m.SchoolTypeId == _schoolTypeId && m.SubTestId == items.SubTestId && m.MinRange <= items.Marks && m.MaxRange >= items.Marks && m.IsActive == true && m.IsDelete == false);

                        var getPerformanceData = _context.Mst_Performance.FirstOrDefault(m => m.MinRange <= getStenScoreData.StenScore && m.MaxRange >= getStenScoreData.StenScore && m.IsActive == true && m.IsDelete == false);

                        Student_Result objResult = new Student_Result();
                        objResult.StudentId = getLastRecordId;
                        objResult.SubTestId = items.SubTestId;
                        objResult.ObtainScore = items.Marks;
                        objResult.StenScore = getStenScoreData.StenScore;
                        objResult.PerformanceScore = getPerformanceData.Description;
                        objResult.NormsId = getStenScoreData.Id;
                        objResult.PerformanceId = getPerformanceData.Id;
                        objResult.IsActive = true;
                        objResult.IsDelete = false;
                        objResult.CreatedOn = Helpers.Utilities.GetCurrentDateTime();
                        _context.Student_Result.Add(objResult);
                        _context.SaveChanges();

                        var getSubTestData = _context.Mst_SubTest.FirstOrDefault(m => m.Id == items.SubTestId && m.IsActive == true && m.IsDelete == false);

                        //graphStenScoreList.Add(Convert.ToInt32(getStenScoreData.StenScore));
                        //graphSubTestList.Add(getSubTestData.Name);
                        _chart.Add(new ChartFields { No = Convert.ToInt32(getStenScoreData.StenScore), Test = getSubTestData.Name });
                    }
                }
               

                List<ChartFields> chartdata = new List<ChartFields>();
                chartdata = _chart;

                var chart = new Chart(width: 600, height: 400)
                    .AddSeries("Default", chartType: "Column",
                                xValue: chartdata, xField: "Test",
                                yValues: chartdata, yFields: "No")
                                .SetYAxis("Sten Score", 0, 10)
                                .SetXAxis("Name of Sub-Test")
                                .AddTitle("")
                                .GetBytes("png");
                string imgpath = "data:image/png;base64," + Convert.ToBase64String(chart);
               var completeGraphImagePath = Helpers.FileUploader.UploadBase64StringToImage("BarChart", imgpath);

                Student_Graph objGraph = new Student_Graph();
                objGraph.StudentId = getLastRecordId;
                objGraph.GraphImagePath = completeGraphImagePath;
                _context.Student_Graph.Add(objGraph);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

           

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == Id);
            CreateEditStudentViewModel viewModel = new CreateEditStudentViewModel();
            var getClassList = _context.Mst_Class.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
           
            var getSchoolList = _context.Mst_School.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            var getGenderList = _context.Mst_Gender.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
           
            var getSubTestList = _context.Mst_SubTest.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();

            viewModel.SelectClassList = new SelectList(getClassList, "Id", "Name", getStudentData.ClassId);
            
            viewModel.SelectSchoolList = new SelectList(getSchoolList, "Id", "Name", getStudentData.SchoolId);
            viewModel.SelectGenderList = new SelectList(getGenderList, "Id", "Name", getStudentData.GenderId);
            viewModel.SubTestModel = getSubTestList;
         
            viewModel.Id = getStudentData.Id;
            viewModel.Name = getStudentData.Name;
            viewModel.FatherName = getStudentData.FatherName;
            viewModel.ContactNo = getStudentData.ContactNo;
            viewModel.ClassId = getStudentData.ClassId;
            viewModel.GenderId = getStudentData.GenderId;
            viewModel.SchoolId = getStudentData.SchoolId;

            var getTestData = _context.Student_Result.Where(m => m.StudentId == getStudentData.Id).OrderBy(m=>m.SubTestId).ToList();
            List<EditSubTestModel> listResult = new List<EditSubTestModel>();
            foreach(var items in getTestData)
            {
                EditSubTestModel obj = new EditSubTestModel();
                obj.TestId = items.SubTestId;
                obj.TestName = items.Mst_SubTest.Name;
                obj.ObtainMarkes = items.ObtainScore;
                listResult.Add(obj);
            }
            viewModel.EditDataSubTestModel = listResult;
            return View(viewModel);

        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        [HttpPost]
        public JsonResult Edit(CreateEditStudentViewModel studentData, ObtainMarks[] subTestData)
        {
            try
            {
                var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == studentData.Id);
               
                getStudentData.Name = studentData.Name;
                getStudentData.FatherName = studentData.FatherName;
                getStudentData.EmailId = studentData.EmailId;
                getStudentData.ContactNo = studentData.ContactNo;
                getStudentData.ClassId = studentData.ClassId;
                getStudentData.SchoolId = studentData.SchoolId;
                getStudentData.GenderId = studentData.GenderId;
                getStudentData.IsActive = true;
                getStudentData.IsVerified = false;
                getStudentData.CreatedOn = Helpers.Utilities.GetCurrentDateTime();
                getStudentData.CreatedBy = User.UserId;
               
                _context.SaveChanges();

                List<ChartFields> _chart = new List<ChartFields>();
                if (subTestData == null)
                {

                }
                else
                {
                    var _schoolTypeId = _context.Mst_School.FirstOrDefault(m => m.Id == studentData.SchoolId).Mst_SchoolType.Id;
                    foreach (var items in subTestData)
                    {
                        var getStenScoreData = _context.Mst_Norms.FirstOrDefault(m => m.ClassId == studentData.ClassId && m.SchoolTypeId == _schoolTypeId && m.SubTestId == items.SubTestId && m.MinRange <= items.Marks && m.MaxRange >= items.Marks && m.IsActive == true && m.IsDelete == false);

                        var getPerformanceData = _context.Mst_Performance.FirstOrDefault(m => m.MinRange <= getStenScoreData.StenScore && m.MaxRange >= getStenScoreData.StenScore && m.IsActive == true && m.IsDelete == false);

                        var studentResultData = _context.Student_Result.FirstOrDefault(m => m.StudentId == getStudentData.Id && m.SubTestId == items.SubTestId);
                        studentResultData.StudentId = getStudentData.Id;
                        studentResultData.SubTestId = items.SubTestId;
                        studentResultData.ObtainScore = items.Marks;
                        studentResultData.StenScore = getStenScoreData.StenScore;
                        studentResultData.PerformanceScore = getPerformanceData.Description;
                        studentResultData.NormsId = getStenScoreData.Id;
                        studentResultData.PerformanceId = getPerformanceData.Id;
                        studentResultData.IsActive = true;
                        //_context.Student_Result.Add(objResult);
                        _context.SaveChanges();

                        var getSubTestData = _context.Mst_SubTest.FirstOrDefault(m => m.Id == items.SubTestId && m.IsActive == true && m.IsDelete == false);

                        //graphStenScoreList.Add(Convert.ToInt32(getStenScoreData.StenScore));
                        //graphSubTestList.Add(getSubTestData.Name);
                        _chart.Add(new ChartFields { No = Convert.ToInt32(getStenScoreData.StenScore), Test = getSubTestData.Name });
                    }
                }


                List<ChartFields> chartdata = new List<ChartFields>();
                chartdata = _chart;

                var chart = new Chart(width: 600, height: 400)
                    .AddSeries("Default", chartType: "Column",
                                xValue: chartdata, xField: "Test",
                                yValues: chartdata, yFields: "No")
                                .SetYAxis("Sten Score", 0, 10)
                                .SetXAxis("Name of Sub-Test")
                                .AddTitle("")
                                .GetBytes("png");
                string imgpath = "data:image/png;base64," + Convert.ToBase64String(chart);
                var completeGraphImagePath = Helpers.FileUploader.UploadBase64StringToImage("BarChart", imgpath);

                var graphData = _context.Student_Graph.FirstOrDefault(m => m.StudentId == getStudentData.Id);

                graphData.StudentId = getStudentData.Id;
                graphData.GraphImagePath = completeGraphImagePath;
                //_context.Student_Graph.Add(objGraph);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }



            return Json(true, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(Roles = "SuperAdministrator, Administrator")]
        [HttpGet]
        public ActionResult View(int Id)
        {
            try
            {

            var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == Id);
            var getGraphData = _context.Student_Graph.FirstOrDefault(m => m.StudentId == getStudentData.Id);


            var testResult = (from a in _context.Mst_Student
                              join b in _context.Student_Result on a.Id equals b.StudentId
                              join c in _context.Mst_SubTest on b.SubTestId equals c.Id
                              where a.Id==Id
                              select new { b, SubTest = c.Name, SubTestHindiName=c.HindiName });


            List<StudentResultModel> _studentResultData = new List<StudentResultModel>();
            foreach(var items in testResult)
            {
                StudentResultModel obj = new StudentResultModel(); 
                if(getStudentData.Mst_School.Mst_SchoolMedium.Id==1)
                {
                    obj.SubTest = items.SubTestHindiName;
                }
                else if(getStudentData.Mst_School.Mst_SchoolMedium.Id == 2)
                {
                    obj.SubTest = items.SubTest;
                   
                }
                else
                {
                    obj.SubTest = items.SubTest;
                }
                obj.ScoreObtained = items.b.ObtainScore;
                obj.StenScore = items.b.StenScore;
                obj.Performance = items.b.PerformanceScore;
                _studentResultData.Add(obj);
            }

            StudentDetailViewModel viewModel = new StudentDetailViewModel();
            viewModel.Id = getStudentData.Id;
            viewModel.StudentResultList = _studentResultData;
            viewModel.BarChartImagePath = Helpers.AppConstants.BaseURL+ getGraphData.GraphImagePath;
            viewModel.Name = getStudentData.Name;
            viewModel.ContactNo = getStudentData.ContactNo;
            viewModel.FatherName = getStudentData.FatherName;
            viewModel.Class = getStudentData.Mst_Class.Name;
            viewModel.SchoolType = getStudentData.Mst_School.Mst_SchoolType.Name;
            viewModel.School = getStudentData.Mst_School.Name;
            viewModel.Gender = getStudentData.Mst_Gender.Name;
            viewModel.Suggestions = getStudentData.Suggestions;
            viewModel.IsVerified = getStudentData.IsVerified;

            return View(viewModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public ActionResult Print(int Id)
        {
            try
            {
            var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == Id);
            var getGraphData = _context.Student_Graph.FirstOrDefault(m => m.StudentId == getStudentData.Id);


            var testResult = (from a in _context.Mst_Student
                              join b in _context.Student_Result on a.Id equals b.StudentId
                              join c in _context.Mst_SubTest on b.SubTestId equals c.Id
                              where a.Id == Id
                              select new { b, SubTest = c.Name, SubTestHindiName = c.HindiName });


            List<StudentResultModel> _studentResultData = new List<StudentResultModel>();
            foreach (var items in testResult)
            {
                StudentResultModel obj = new StudentResultModel();
                if (getStudentData.Mst_School.Mst_SchoolMedium.Id == 1)
                {
                    obj.SubTest = items.SubTestHindiName +" ("+ items.SubTest +") ";
                }
                else if (getStudentData.Mst_School.Mst_SchoolMedium.Id == 2)
                {
                    obj.SubTest = items.SubTest;

                }
                else
                {
                    obj.SubTest = items.SubTest;
                }
                obj.ScoreObtained = items.b.ObtainScore;
                obj.StenScore = items.b.StenScore;
                obj.Performance = items.b.PerformanceScore;
                _studentResultData.Add(obj);
            }

            StudentDetailViewModel viewModel = new StudentDetailViewModel();

            //var getPdfPath = _context.Student_Pdf.Where(m => m.StudentId == Id).OrderByDescending(m => m.Id).Take(1).FirstOrDefault();
            //if (getPdfPath != null)
            //{
            //    viewModel.IsPdfPath = true;
            //    viewModel.PdfPath = Helpers.AppConstants.BaseURL + getPdfPath.FilePath;
            //}
            //else
            //{
            //    viewModel.IsPdfPath = false;
            //    viewModel.PdfPath = "";
            //}


            viewModel.Id = getStudentData.Id;
            viewModel.StudentResultList = _studentResultData;
            viewModel.BarChartImagePath = Helpers.AppConstants.BaseURL + getGraphData.GraphImagePath;
            viewModel.Name = getStudentData.Name;
            viewModel.ContactNo = getStudentData.ContactNo;
            viewModel.FatherName = getStudentData.FatherName;
            viewModel.Class = getStudentData.Mst_Class.Name;
            viewModel.SchoolType = getStudentData.Mst_School.Mst_SchoolType.Name;
            viewModel.School = getStudentData.Mst_School.Name;
            viewModel.Gender = getStudentData.Mst_Gender.Name;
            viewModel.Suggestions = getStudentData.Suggestions;
                viewModel.SchoolMediumId = getStudentData.Mst_School.Mst_SchoolMedium.Id;

            return View(viewModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator")]
        public ActionResult SaveSuggeation(StudentDetailViewModel values)
        {
            var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == values.Id);
            getStudentData.Suggestions = values.Suggestions;
            getStudentData.IsVerified = true;
            _context.SaveChanges();
            return RedirectToAction("View", new { id = getStudentData.Id });
        }


        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        public ActionResult ChangeStatus(int id)
        {

            var getData = _context.Mst_Student.FirstOrDefault(m => m.Id == id);
            if (getData.IsActive == true)
            {
                getData.IsActive = false;

            }
            else if (getData.IsActive == false)
            {
                getData.IsActive = true;

            }
            else
            {
                getData.IsActive = false;

            }
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
        public ActionResult Delete(int id)
        {
            var getRecordsForDelete = _context.Mst_Student.FirstOrDefault(m => m.Id == id);
            getRecordsForDelete.IsDelete = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetSchoolType(int SchoolId)
        {
            var getData = _context.Mst_School.FirstOrDefault(m => m.Id == SchoolId);
            GetSchooDetailsData obj = new GetSchooDetailsData();
            obj.SchoolMedium = getData.Mst_SchoolMedium.Name;
            obj.SchoolTypeName = getData.Mst_SchoolType.Name;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public class ChartModel
        {
            public List<ChartFields> ChartData { get; set; }
        }
        public class ChartFields
        {
            public int No { get; set; }
            public string Test { get; set; }
        }

        #region PageHelper
        private SelectList GetClassSelectList()
        {
            var getData = _context.Mst_Class.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            return new SelectList(getData, "Id", "Name");
        }
        private SelectList GetSchoolSelectList()
        {
            var getData = _context.Mst_School.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            return new SelectList(getData, "Id", "Name");
        }
        private SelectList GetGenderSelectList()
        {
            var getData = _context.Mst_Gender.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            return new SelectList(getData, "Id", "Name");
        }
        private SelectList GetSubTestSelectList()
        {
            var getData = _context.Mst_SubTest.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            return new SelectList(getData, "Id", "Name");
        }

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
        #endregion

    }
}