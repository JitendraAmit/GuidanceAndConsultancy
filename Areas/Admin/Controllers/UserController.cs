using GuidanceConsultancy.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Areas.Admin.Models.UserViewModel;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;

namespace GuidanceConsultancy.Areas.Admin.Controllers
{
    [CustomHandleException]
    [CustomAuthorize(Roles = "SuperAdministrator")]
    public class UserController : BaseController
    {
        public readonly DishaGuidanceEntities _context;

        public UserController()
        {
            _context = new DishaGuidanceEntities();
        }
        // GET: Admin/School
        public ActionResult Index()
        {
            var getData = _context.Mst_Login.Where(m => m.IsDelete == false && m.RoleId!=1).OrderByDescending(m => m.Id).ToList();
            UserListViewModel viewModel = new UserListViewModel();
            viewModel.UserModel = getData;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateEditUserViewModel viewModel = new CreateEditUserViewModel();
            viewModel.SelectRoleList = GetRoleSelectList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateEditUserViewModel values)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var PasswordMd5Hash = Utilities.CreateMD5HashFromPlainString(values.Pwd);
                    Mst_Login obj = new Mst_Login();
                    obj.Name = values.Name;
                    obj.UserName = values.UserName;
                    obj.Pwd = PasswordMd5Hash;
                    obj.RoleId = values.RoleId;
                    obj.IsActive = true;
                    obj.IsDelete = false;
                    obj.CreatedOn = Utilities.GetCurrentDateTime();
                    _context.Mst_Login.Add(obj);
                    _context.SaveChanges();
                }
                else
                {
                    values.SelectRoleList = GetRoleSelectList();
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
            var getData = _context.Mst_Login.FirstOrDefault(m => m.Id == Id);
            CreateEditUserViewModel viewModel = new CreateEditUserViewModel();
            var getRoleList = _context.Mst_Role.Where(m => m.IsActive == true && m.IsDelete == false && m.Id != 1).OrderBy(m => m.Name).ToList();
            viewModel.SelectRoleList = new SelectList(getRoleList, "Id", "Name", getData.RoleId);
            viewModel.Id = getData.Id;
            viewModel.Name = getData.Name;
            viewModel.RoleId = getData.RoleId;
            viewModel.UserName = getData.UserName;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(CreateEditUserViewModel values)
        {
            try
            {
                if (!string.IsNullOrEmpty(values.Name) && values.RoleId >0)
                {
                    
                    var getData = _context.Mst_Login.FirstOrDefault(m => m.Id == values.Id);
                    getData.Name = values.Name;
                    getData.RoleId = values.RoleId;
                    getData.IsActive = true;
                    _context.SaveChanges();
                }
                else
                {
                    values.SelectRoleList = GetRoleSelectList();
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

        public JsonResult UserAlreadyExist(string UserName)
        {
            bool chk = _context.Mst_Login.Any(m => m.UserName == UserName);
            if (chk == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private SelectList GetRoleSelectList()
        {
            var getData = _context.Mst_Role.Where(m => m.IsActive == true && m.IsDelete == false && m.Id != 1).OrderBy(m => m.Name).ToList();
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