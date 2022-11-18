using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;
using GuidanceConsultancy.Areas.Admin.Models.SchoolViewModel;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;

namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
    [CustomAuthorize(Roles = "SuperAdministrator, Administrator")]
    public class SchoolController : BaseController
    {
        public readonly DishaGuidanceEntities _context;

        public SchoolController()
        {
            _context = new DishaGuidanceEntities();
        }
        // GET: Admin/School
        public ActionResult Index()
        {
            var getData = _context.Mst_School.Where(m =>m.IsDelete == false).OrderByDescending(m => m.Id).ToList();
            SchoolListViewModel viewModel = new SchoolListViewModel();
            viewModel.SchoolModel = getData;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateEditSchoolViewModel viewModel = new CreateEditSchoolViewModel();
            viewModel.SelectSchoolTypeList = GetSchoolTypeSelectList();
            viewModel.SelectSchoolMediumList = GetSchoolMediumSelectList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateEditSchoolViewModel values)
        {
            try
            {
               if(ModelState.IsValid)
               {
                   Mst_School obj = new Mst_School();
                   obj.Name = values.Name;
                   obj.SchoolTypeId = values.SchoolTypeId;
                   obj.SchoolMediumId = values.SchoolMediumId;
                   obj.IsActive = true;
                   obj.IsDelete = false;
                   obj.CreatedOn = Utilities.GetCurrentDateTime();
                   _context.Mst_School.Add(obj);
                   _context.SaveChanges();
               }
               else
               {
                    return View(values);
               }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var getData = _context.Mst_School.FirstOrDefault(m => m.Id == Id);
            CreateEditSchoolViewModel viewModel = new CreateEditSchoolViewModel();
            var getSchoolTypeList = _context.Mst_SchoolType.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            viewModel.SelectSchoolTypeList = new SelectList(getSchoolTypeList, "Id", "Name", getData.SchoolTypeId);
            var getSchoolMediumList = _context.Mst_SchoolMedium.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            viewModel.SelectSchoolMediumList = new SelectList(getSchoolMediumList, "Id", "Name", getData.SchoolMediumId);

            viewModel.Id = getData.Id;
            viewModel.Name = getData.Name;
            viewModel.SchoolTypeId = getData.SchoolTypeId;
            viewModel.SchoolMediumId = getData.SchoolMediumId;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(CreateEditSchoolViewModel values)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getData = _context.Mst_School.FirstOrDefault(m => m.Id == values.Id);
                    getData.Name = values.Name;
                    getData.SchoolTypeId = values.SchoolTypeId;
                    getData.SchoolMediumId = values.SchoolMediumId;
                    getData.IsActive = true;
                    _context.SaveChanges();
                }
                else
                {
                    return View(values);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatus(int id)
        {

            var getData = _context.Mst_School.FirstOrDefault(m => m.Id == id);
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

       
        public ActionResult Delete(int id)
        {
            var getRecordsForDelete = _context.Mst_School.FirstOrDefault(m => m.Id == id);
            getRecordsForDelete.IsDelete = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        #region PageHelper
        private SelectList GetSchoolTypeSelectList()
        {
            var getData = _context.Mst_SchoolType.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            return new SelectList(getData, "Id", "Name");
        }

        private SelectList GetSchoolMediumSelectList()
        {
            var getData = _context.Mst_SchoolMedium.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
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