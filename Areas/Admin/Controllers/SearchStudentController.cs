using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;
using GuidanceConsultancy.Areas.Admin.Models.SearchViewModel;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;
using System.Data.Entity;

namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
    [CustomAuthorize(Roles = "SuperAdministrator, Administrator")]
    public class SearchStudentController : BaseController
    {
        public DishaGuidanceEntities _context;
        public SearchStudentController()
        {
            _context = new DishaGuidanceEntities();
        }

        // GET: Admin/SearchStudent
        [HttpGet]
        public ActionResult Index()
        {
            SearchDataModel viewModel = new SearchDataModel();
            var getUserList = _context.Mst_Login.Where(m => m.RoleId != 1).OrderBy(m => m.Id).ToList();
            viewModel.SelectUserList = new SelectList(getUserList, "Id", "Name");
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult Search(SearchDataModel values)
        {
            DateTime todayDate = Utilities.GetCurrentDateTime();
            var _Name = (!string.IsNullOrEmpty(values.Name))?values.Name:"";
            var _ContactNo = (!string.IsNullOrEmpty(values.ContactNo)) ? values.ContactNo : "";
            int ? _CreatedBy = (values.CreatedById>0) ? values.CreatedById : 0;
            var _SDate = (!string.IsNullOrEmpty(values.StartDate)) ? values.StartDate : "";
            var _EDate = (!string.IsNullOrEmpty(values.EndDate)) ? values.EndDate : "";

            SearchDataModel viewModel = new SearchDataModel();

            //var getfilterData = (from a in _context.Mst_Student
            //               join b in _context.Mst_Gender on a.GenderId equals b.Id
            //               join c in _context.Mst_Class on a.ClassId equals c.Id
            //               join d in _context.Mst_School on a.SchoolId equals d.Id
            //               where a.IsDelete==false select new { a, GenderName = b.Name, ClassName = c.Name, SchoolName = d.Name }).OrderByDescending(m=>m.a.Id).Skip(values.PageLimit * (values.PageNo - 1))
            //               .Take(values.PageLimit);

            var getfilterData = (from a in _context.Mst_Student
                                 join b in _context.Mst_Gender on a.GenderId equals b.Id
                                 join c in _context.Mst_Class on a.ClassId equals c.Id
                                 join d in _context.Mst_School on a.SchoolId equals d.Id
                                 where a.Name==_Name || a.ContactNo==_ContactNo || a.CreatedBy==_CreatedBy && a.IsDelete == false 
                                 select new { a, GenderName = b.Name, ClassName = c.Name, SchoolName = d.Name }).OrderByDescending(m => m.a.Id).Skip(values.PageLimit * (values.PageNo - 1)).Take(values.PageLimit);

            //if (!string.IsNullOrEmpty(values.Name))
            //{
            //    getfilterData = getfilterData.Where(m => m.a.Name == values.Name);
            //}
            //else if (!string.IsNullOrEmpty(values.ContactNo))
            //{
            //    getfilterData = getfilterData.Where(m => m.a.ContactNo == values.ContactNo);
            //}
            //else if (values.CreatedById != null && values.CreatedById > 0)
            //{
            //    getfilterData = getfilterData.Where(m => m.a.CreatedBy == values.CreatedById);
            //}
            //else if (!string.IsNullOrEmpty(values.StartDate) && !string.IsNullOrEmpty(values.EndDate))
            //{
            //    DateTime sDate = Convert.ToDateTime(values.StartDate);
            //    DateTime eDate = Convert.ToDateTime(values.EndDate);
            //    getfilterData = getfilterData.Where(m => DbFunctions.TruncateTime(m.a.CreatedOn) >= DbFunctions.TruncateTime(sDate) && DbFunctions.TruncateTime(m.a.CreatedOn) <= DbFunctions.TruncateTime(eDate));
            //}
            //else if (!string.IsNullOrEmpty(values.StartDate) || !string.IsNullOrEmpty(values.EndDate))
            //{
            //    if (!string.IsNullOrEmpty(values.StartDate))
            //    {
            //        DateTime sdate = Convert.ToDateTime(values.StartDate);
            //        getfilterData = getfilterData.Where(m => DbFunctions.TruncateTime(m.a.CreatedOn) == DbFunctions.TruncateTime(sdate));
            //    }
            //    else if (!string.IsNullOrEmpty(values.EndDate))
            //    {
            //        DateTime edate = Convert.ToDateTime(values.EndDate);
            //        getfilterData = getfilterData.Where(m => DbFunctions.TruncateTime(m.a.CreatedOn) == DbFunctions.TruncateTime(edate));
            //    }
            //}

            

            List<StudentListViewModel> listStudent = new List<StudentListViewModel>();
            foreach(var items in getfilterData)
            {
                StudentListViewModel obj = new StudentListViewModel();
                obj.Id = items.a.Id;
                obj.Name = items.a.Name;
                obj.FatherName = items.a.FatherName;
                obj.ContactNo = items.a.ContactNo;
                obj.Gender = items.GenderName;
                obj.Class = items.ClassName;
                obj.School = items.SchoolName;
                listStudent.Add(obj);
            }
            viewModel.StudentListModel = listStudent;

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }



        #region Dispose
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
        #endregion

    }
}