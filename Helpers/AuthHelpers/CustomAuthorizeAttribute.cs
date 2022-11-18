using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GuidanceConsultancy.Helpers.AuthHelpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
             get { return HttpContext.Current.User as CustomPrincipal; }
            
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        //
                        // redirect to AccessDenied page
                        //
                        var result = new ViewResult { ViewName = "_AccessDenied" };
                        filterContext.Result = result;

                        /*

                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new
                        {
                            controller = "Error",
                            action = "AccessDenied",
                            area = ""
                            //returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                        }));
                        */

                        //base.OnAuthorization(filterContext); //returns to login url
                    }
                }

                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrentUser.UserId.ToString()))
                    {
                        //

                        //

                        //filterContext.Result = new RedirectToRouteResult(new
                        //RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));

                        // base.OnAuthorization(filterContext); //returns to login url
                    }
                }
            }
            else
            {
                //filterContext.Result = new RedirectToRouteResult(new
                //        RouteValueDictionary(new { controller = "Account", action = "Login" }));
                // base.OnAuthorization(filterContext); //returns to login url

                //redirecting to login page with return url...
                filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    controller = "Account",
                                    action = "Login",
                                    area = "",
                                    returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                                }));
            }

        }
    }
}