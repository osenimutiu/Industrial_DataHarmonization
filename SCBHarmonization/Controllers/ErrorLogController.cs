using Microsoft.AspNet.Identity;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SCBHarmonization.Controllers
{

    //[ExceptionHandler]
    [Authorize]
    public class ErrorLogController : Controller
    {
        private SCBDBEntities obj;
        public ErrorLogController()
        {
            obj = new SCBDBEntities();
        }
        // GET: ErrorLog
        public ActionResult Index()
        {

            List<ExceptionLogger> x = obj.ExceptionLoggers.OrderByDescending(s => s.LogTime).Take(15).ToList();
            return View(x);
        }

        public ActionResult FixError(int id)
        {
            var fix = obj.ExceptionLoggers.Find(id);
            obj.ExceptionLoggers.Remove(fix);
            obj.SaveChanges();
            TempData["SuccessMessage"] = "Fix Successfully";
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} fix error successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            obj.TBL_AUDIT.Add(audit);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
        //[HttpGet]
        //public JsonResult FixError(int id)
        //{
        //    var fix = obj.ExceptionLoggers.Find(id);
        //    obj.ExceptionLoggers.Remove(fix);
        //    obj.SaveChanges();
        //    string message = "Deleted Successfully";
        //    return Json(new {Status = 1, Message = message }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetExceptionHistory()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
            if (cnnString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                cnnString = efBuilder.ProviderConnectionString;
            }
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_fixexception";
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            ViewBag.Message = "Exception Fixed Successfully";
            cnn.Close();
            return RedirectToAction("Index");
        }

        public ActionResult GetErrorList()
        {
            var res = obj.ELMAH_Error.OrderByDescending(x => x.TimeUtc).ThenBy(y => y.ErrorId).Take(5).ToList();
            return View(res);
        }
    }
}