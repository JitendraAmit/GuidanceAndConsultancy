using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GuidanceConsultancy.Helpers;
using GuidanceConsultancy.Helpers.AuthHelpers;
using GuidanceConsultancy.Models.AccountViewModel;
using GuidanceConsultancy.Models.db;
using Newtonsoft.Json;

namespace GuidanceConsultancy.Controllers
{
    public class AccountController : Controller
    {
        public readonly DishaGuidanceEntities _context;

        public AccountController()
        {
            _context = new DishaGuidanceEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //[Route("Login")]
        //[Route("Account/Login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountDataModel values, string ReturnUrl = "")
        {
            if (string.IsNullOrEmpty(values.UserName))
            {
                ModelState.AddModelError(string.Empty, "Username can not be empty.");
                return View();
            }

            if (string.IsNullOrEmpty(values.Password))
            {
                ModelState.AddModelError(string.Empty, "Password can not be empty.");
                return View();
            }

            Mst_Login user;

            string HashedPassword = Utilities.CreateMD5HashFromPlainString(values.Password);
            user = _context.Mst_Login.FirstOrDefault(u => u.UserName == values.UserName && u.Pwd == HashedPassword);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            if (user.IsActive == false)
            {
                ModelState.AddModelError(string.Empty, "Inactive account please contact to admin");
                return View();
            }

            if (user.IsDelete == true)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }




            //if ((user.IsActive ?? false) == false)
            //{
            //    ModelState.AddModelError(string.Empty, "Account not active.");
            //    return View();
            //}

            //string[] roles = _context.TblRole.Select(m => m.Role).ToArray();

            //string[] roles = _context.TblUser.Select(m => m.TblRole.Role).Where(m).ToArray();
            string[] roles = _context.Mst_Login.Where(m => m.Id == user.Id).Select(m => m.Mst_Role.Name).ToArray();

            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = user.Id;
            serializeModel.UserName = user.UserName;
            serializeModel.DisplayName = user.Name;
            //serializeModel.AvatarURL = user.ProfileImage;
            serializeModel.Roles = roles;

            string userData = JsonConvert.SerializeObject(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.Now,
                DateTime.Now.AddMinutes(1440),
                true,
                userData);


            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }



        }




        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            // HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            return RedirectToAction("Login");
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