using Microsoft.AspNet.Identity;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class SendMailerController : Controller
    {
        // GET: SendMailer
        private SCBDBEntities db;
        public SendMailerController()
        {
            db = new SCBDBEntities();
        }
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ViewResult Index(MailViewModel _objModelMail, string message)
        {
            if (ModelState.IsValid)
            {
                //added
                //_objModelMail.Subject = "Email Testing";
                //_objModelMail.Body = "Email Testing from Mutiu";
                //_objModelMail.AdditionalComment = "Additional Comment";
                MailMessage mail = new MailMessage();
                //mail.To.Add("abiodun.oyekunle@fintraksoftware.com");
                mail.To.Add("mutiuakanni@zohomail.com");
                mail.From = new MailAddress("osenimutiu1@yahoo.com");
                mail.Subject = _objModelMail.Subject;
                string comment = _objModelMail.AdditionalComment;
                string Body = "Dear Fintrak Support, <br><br> This error was encountered while running Data Harmonization application. <br><br> " + _objModelMail.Body + "<br><br>Additional Comment <br> " + comment + "<br><br> Regards, <br><br>Standard Chartered Bank.";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.mail.yahoo.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("osenimutiu1@yahoo.com", "lurhlqwkdlorjicc");  
                smtp.EnableSsl = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                smtp.Send(mail);
                var audit = new TBL_AUDIT()
                {
                    URL = Request.Url.AbsoluteUri,
                    UserName = User.Identity.Name,
                    DETAIL = $"User {User.Identity.Name} sent complain to FinTrak support",
                    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    DEVICENAME = SendEmail.GetDeviceName(),
                    OSNAME = SendEmail.FriendlyName(),
                    DATETIME = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId()
                };
                db.TBL_AUDIT.Add(audit);
                db.SaveChanges();
                ViewBag.Message = string.Format("Mail sent Successfully {0}", message, DateTime.Now.ToString());
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetExceptionForSend()
        {
            var i = 1;
            var dataList = db.ExceptionLoggers.OrderByDescending(s => s.Id).Take(1).ToList();
            var data = dataList.Select(x => new
            {
                Id = x.Id,
                ExceptionMessage = x.ExceptionMessageWithSolution,
                ControllerName = x.ControllerName,
                ExceptionStackTrace = x.ExceptionStackTrace,
                LogTime = x.LogTime,
                ActionName = x.ActionName,
                InnerException = x.InnerException,

            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExceptionForDropdown()
        {
            int i = 5;
            var dataList = db.ExceptionLoggers.OrderByDescending(s => s.Id).Take(i).ToList();
            var data = dataList.Select(x => new
            {
                Id = x.Id,
                ExceptionMessage = x.ExceptionMessageWithSolution,
                ControllerName = x.ControllerName,
                ExceptionStackTrace = x.ExceptionStackTrace,
                LogTime = x.LogTime,
                ActionName = x.ActionName,
                InnerException = x.InnerException,

            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}