using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;
using GuidanceConsultancy.Areas.Admin.Models.NormsViewModel;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;

namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
    [CustomAuthorize(Roles = "Administrator")]
    public class NormsController : BaseController
    {
        public readonly DishaGuidanceEntities _context;

        public NormsController()
        {
            _context = new DishaGuidanceEntities();
        }
        // GET: Admin/Norms
        public ActionResult Index()
        {
            var getData = _context.Mst_Norms.Where(m => m.IsDelete == false).OrderByDescending(m => m.Id).ToList();
            NormsListViewModel viewModel = new NormsListViewModel();
            viewModel.NormsModel = getData;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateEditNormsViewModel viewModel = new CreateEditNormsViewModel();
            var getClassList = _context.Mst_Class.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            var getSchoolTypeList = _context.Mst_SchoolType.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();
            var getSubTestList = _context.Mst_SubTest.Where(m => m.IsActive == true && m.IsDelete == false).OrderBy(m => m.Id).ToList();

            viewModel.SelectClassList = new SelectList(getClassList, "Id", "Name");
            viewModel.SelectSchoolTypeList = new SelectList(getSchoolTypeList, "Id", "Name");
            viewModel.SelectSubTestList = new SelectList(getSubTestList, "Id", "Name");
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(CreateEditNormsViewModel values)
        {
            try
            {
                Mst_Norms obj = new Mst_Norms();
                obj.ClassId = values.ClassId;
                obj.SchoolTypeId = values.SchoolTypeId;
                obj.SubTestId = values.SubTestId;
                obj.MinRange = values.MinRange;
                obj.MaxRange = values.MaxRange;
                obj.StenScore = values.StenScore;
                obj.IsActive = true;
                obj.IsDelete = false;
                obj.CreatedOn = Utilities.GetCurrentDateTime();
                _context.Mst_Norms.Add(obj);
                _context.SaveChanges();
                
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
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