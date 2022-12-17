using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCBHarmonization.CustomFilter
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessageWithSolution = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    ActionName = filterContext.RouteData.Values["action"].ToString(),
                    InnerException = filterContext.Exception.InnerException.Message,
                    LogTime = DateTime.Now

                };
                string TargetSite = filterContext.Exception.InnerException.Source;
                string Errorb = filterContext.Exception.InnerException.HelpLink;
                SCBDBEntities ctx = new SCBDBEntities();
                ctx.ExceptionLoggers.Add(logger);
                ctx.SaveChanges();
                ExceptionHistory objHistory = new ExceptionHistory();
                objHistory.UpdateExceptionHistory();
                filterContext.ExceptionHandled = true;

                if (filterContext.ExceptionHandled)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Home" },
                            {"action", "Error" }
                        });
                }
            }
        }
    }
}