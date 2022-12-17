using Microsoft.AspNet.Identity;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{

    public class UserController : Controller
    {
        private readonly SCBDBEntities objDbEntities;
        public UserController()
        {
            objDbEntities = new SCBDBEntities();
        }
        //[AuthorizationFilter]
        [HttpGet]
        // GET: User
        public ActionResult Index()
        {
            //SCBDBEntities objDbEntities = new SCBDBEntities();
            //Tbl_UserVM obj = new Tbl_UserVM();
            //obj.ListOfRoles = (from t in objDbEntities.Tbl_Roles
            //                                           select new SelectListItem()
            //                                           {
            //                                               Text = t.RoleName,
            //                                               Value = t.RoleName.ToString()
            //                                           }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Index(Tbl_UserVM objuser)
        {
            SCBDBEntities db = new SCBDBEntities();
            bool isSuccess = true;
            string message = string.Empty;
            //if (objuser.UserId > 0)
            //{
            //    db.Entry(objuser).State = EntityState.Modified;
            //}
            //else
            //{
            //user.Password = AppHelper.GetMd5Hash(user.Password);
            //var psw = AppHelper.GetMd5Hash(user.Password);
            //user.Password = psw;
            var pass = AppHelperMVC.GetMd5Hash(objuser.Password);
            if (db.Tbl_Users.Any(e => e.Email == objuser.Email))
            {
                //ModelState.AddModelError("", "Email " + objuser.Email + "Exist!");
                //return View(objuser);
                //ViewBag.ExistMessage = "Email " + objuser.Email + " Exist!";
                //return View(objuser);
                message = "Email " + objuser.Email + " Exist!";
            }
            if(objuser.Password != objuser.ConfirmPassword)
            {
                //ModelState.AddModelError("", "Password Mismatch");
                //return View(objuser);
                //ViewBag.PasswordError = "Password Mismatch!";
                //return View(objuser);
                message = "Password Mismatch!";
            }
            if (objuser.Role == null || objuser.Role == "")
            {
                objuser.Role = "User".ToLower();
            }
            else
            {
                objuser.Role = objuser.Role.ToLower();
            }
            if (objuser.UserId == 0)
            {
                Tbl_Users obj = new Tbl_Users();
                obj.Email = objuser.Email;
                obj.Password = pass;
                db.Tbl_Users.Add(obj);
                db.SaveChanges();
                message = "Registered Successfully";
                //ViewBag.SuccessMessage = "Registered Successfully";
                //return View();
            }
            else
            {
                db.Entry(objuser).State = EntityState.Modified;
            }
            //return Json(isSuccess, JsonRequestBehavior.AllowGet);
            return Json(new { message = message, success = true }, JsonRequestBehavior.AllowGet);
            //return View();
            //if (objuser.UserId > 0)
            //{
            //    db.Entry(objuser).State = EntityState.Modified;
            //}
            //else
            //{
            //    user.Password = AppHelperMVC.GetMd5Hash(user.Password);
            //    //var psw = AppHelper.GetMd5Hash(user.Password);
            //    //user.Password = psw;
            //    //user.Role = "Not Assigned";
            //    db.Tbl_Users.Add(user);
            //}
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (Exception)
            //{
            //    isSuccess = false;
            //}

            //return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccessDenied()
        {
            return View();
        }

        //[HttpGet]
        //public JsonResult GetAllRoles()
        //{
        //    SCBDBEntities db = new SCBDBEntities();
        //    var dataList = db.Tbl_Roles.ToList();
        //    var data = dataList.Select(x => new
        //    {
        //        Id = x.RoleId,
        //        Name = x.RoleName
        //    });
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        [Authorize]
        public ActionResult ShowAllUserDetails()
        {
            if(User.Identity.IsAuthenticated && User.IsInRole("System Admin"))
            {
                UserViewModel objCustomer = new UserViewModel();
                DataAccessLayer objDB = new DataAccessLayer(); //calling class DBdata    
                objCustomer.ShowallUsers = objDB.SelectAllUsers();
                return View(objCustomer);
            }
            return RedirectToAction("Login", "Account");
           
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeactivateUser(string Id)
        {
            DataAccessLayer objDB = new DataAccessLayer();
            int result = objDB.Deactivate(Id);
            string userName = objDB.SelectDatabyID(Id);
            TempData["result3"] = result;
            ModelState.Clear();
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} Deactivated user {userName} successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return RedirectToAction("ShowAllUserDetails");
        }
    }
}