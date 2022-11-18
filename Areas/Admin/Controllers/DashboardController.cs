using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;
using System.Data.Entity;
using GuidanceConsultancy.Areas.Admin.Models.DashboardViewModel;

namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
    [CustomAuthorize(Roles = "SuperAdministrator, Administrator, User")]
    public class DashboardController : BaseController
    {
        public readonly DishaGuidanceEntities _context;
        public DashboardController()
        {
            _context = new DishaGuidanceEntities();
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            DashboardListModel viewModel = new DashboardListModel();
            if (User.Identity.IsAuthenticated)
            {
                DateTime todayDate = Utilities.GetCurrentDateTime();
                if (User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.SuperAdministrator.RoleName) || User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.Administrator.RoleName))
                {
                    
                    var countTotalStudent = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false).ToList().Count();

                    var countTodayStudent = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false && DbFunctions.TruncateTime(m.CreatedOn) == DbFunctions.TruncateTime(todayDate)).ToList().Count();

                    var totalSchool = _context.Mst_School.Where(m => m.IsActive == true && m.IsDelete == false).ToList().Count();
                    var totalInactiveSchool = _context.Mst_School.Where(m => m.IsActive == false && m.IsDelete == false).ToList().Count();

                    var countTotalUser = _context.Mst_Login.Where(m => m.IsActive == true && m.IsDelete == false && m.RoleId == 2).ToList().Count();

                    var countTotalInactiveUser = _context.Mst_Login.Where(m => m.IsActive == false && m.IsDelete == false && m.RoleId == 2).ToList().Count();

                    ViewBag.totalStudent = countTotalStudent;
                    ViewBag.todaysStudent = countTodayStudent;

                    ViewBag.totalSchool = totalSchool;
                    ViewBag.totalInactiveSchool = totalInactiveSchool;
                    ViewBag.totalUser = countTotalUser;
                    ViewBag.totalInactiveUser = countTotalInactiveUser;

                    var getStudentData = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false).OrderByDescending(m => m.Id).Take(20).ToList();
                    
                    viewModel.StudentModel = getStudentData;
                }
               
                else if (User.IsInRole(GuidanceConsultancy.Helpers.AppConstants.UserRoles.User.RoleName))
                {

                    var countTotalStudentByUser = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false && m.CreatedBy==User.UserId).ToList().Count();

                    var countTodayStudentByUser = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false && m.CreatedBy==User.UserId && DbFunctions.TruncateTime(m.CreatedOn) == DbFunctions.TruncateTime(todayDate)).ToList().Count();
                    ViewBag.totalStudentByUser = countTotalStudentByUser;
                    ViewBag.todaysStudentByUser = countTodayStudentByUser;

                    var getStudentData = _context.Mst_Student.Where(m => m.IsActive == true && m.IsDelete == false && m.CreatedBy==User.UserId).OrderByDescending(m => m.Id).Take(20).ToList();
                  
                    viewModel.StudentModel = getStudentData;
                }
                else
                {

                }
            }
           

            return View(viewModel);
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