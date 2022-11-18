using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Helpers
{
    public class CustomHandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var exception = filterContext.Exception;
                var message = filterContext.Exception.Message;
                var stackTrace = filterContext.Exception.StackTrace;

                //log exception here...
                Logger.LogException(exception);

                var result = new ViewResult { ViewName = "_Error" };

                var modelMetadata = new EmptyModelMetadataProvider();
                result.ViewData = new ViewDataDictionary();

                result.ViewData.Add("Exception", exception);

                var controllerName = filterContext.RouteData.Values["controller"];
                var actionName = filterContext.RouteData.Values["action"];

                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}