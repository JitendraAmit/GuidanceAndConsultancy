using GuidanceConsultancy.Helpers.AuthHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace GuidanceConsultancy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                //getting cookies
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                //var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    //decrypting auth ticket
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null && !authTicket.Expired)
                    {
                        //binding to serialize model
                        CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);

                        //binding serialize model to CustomPrincipal
                        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                        newUser.UserId = serializeModel.UserId;
                        newUser.UserName = serializeModel.UserName;
                        newUser.DisplayName = serializeModel.DisplayName;
                        //newUser.AvatarURL = serializeModel.AvatarURL;
                        newUser.Roles = serializeModel.Roles;

                        //NOTE:If you want to pass more data as user data, assign additional property to custom principal from serialize model

                        //assigning CustomPrincipal to Identity User
                        HttpContext.Current.User = newUser;
                    }


                }
            }
            catch (CryptographicException)
            {
                //Logger.LogException(CryptographicException);
                FormsAuthentication.SignOut();
            }
        }
    }

}
