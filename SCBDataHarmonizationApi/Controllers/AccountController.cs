using SCBDataHarmonizationApi.Helper;
using SCBDataHarmonizationApi.Models;
using SCBDataHarmonizationApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using static SCBDataHarmonizationApi.Helper.AppHelper;

namespace SCBDataHarmonizationApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        //[HttpPost]
        //[Route("register")]
        //public ReturnMessage AddUser(Tbl_UserVM objuser)
        //{
        //    SCBDBEntities db = new SCBDBEntities();
        //    bool isSuccess = true;
        //    var pass = AppHelper.GetMd5Hash(objuser.Password);
        //    if(db.Tbl_Users.Any(e=>e.Email == objuser.Email))
        //    {
        //        return new ReturnMessage { Success = false, Message = "Email " + objuser.Email + " Exists!" };
        //    }
        //    if (objuser.Role == null || objuser.Role == "" || objuser.Role == "string")
        //    {
        //        objuser.Role = "User".ToLower();
        //    }
        //    else
        //    {
        //        objuser.Role = objuser.Role.ToLower();
        //    }
        //    if (objuser.UserId == 0)
        //    {
        //        Tbl_Users obj = new Tbl_Users();
        //        obj.FirstName = objuser.FirstName;
        //        obj.LastName = objuser.LastName;
        //        obj.Email = objuser.Email;
        //        obj.Password = pass;
        //        obj.Role = objuser.Role;
        //        db.Tbl_Users.Add(obj);
        //        db.SaveChanges();
        //        return new ReturnMessage { Success = true, Message = "Registered Successfully!" };
        //    }
        //    return new ReturnMessage();
        //}
        [HttpPost]
        [Route("register")]
        public ReturnMessage AddUser(string email)
        {
            //try
            //{
            SCBDBEntities db = new SCBDBEntities();
            bool isSuccess = true;
            //var pass = AppHelper.GetMd5Hash(objuser.Password);
            //if (!db.Tbl_BankStaff.Any(e => e.Email == email))
            //{
            //    return new ReturnMessage { Success = false, Message = "Staff with Email " + email + " does not exists!" };
            //}
            if (db.Tbl_Users.Any(e => e.Email == email))
            {
                return new ReturnMessage { Success = false, Message = "Email " + email + " Exists!" };
            }
                //if(email != "" || email != null)
                //{
            Tbl_Users obj = new Tbl_Users();
            obj.Email = email;
            var pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
            var encrypPass = AppHelper.GetMd5Hash(pass);
            obj.Password = encrypPass;
            obj.IsActive = true;
            db.Tbl_Users.Add(obj);
            db.SaveChanges();
            ProfileUser(email);
            return new ReturnMessage { Success = true, Message = "Registered Successfully!" };
            //    }
            //return new ReturnMessage();

            //}

            //catch (Exception ex)
            //{

            //}

        }

        [HttpPost]
        [Route("addStaff")]
        public ReturnMessage AddUser(Tbl_BankStaffVM objuser)
        {
            SCBDBEntities db = new SCBDBEntities();
            bool isSuccess = true;
            if (db.Tbl_BankStaff.Any(e => e.Email == objuser.Email))
            {
                return new ReturnMessage { Success = false, Message = "Email " + objuser.Email + " Exists!" };
            }
            if (objuser.Id == 0)
            {
                Tbl_BankStaff obj = new Tbl_BankStaff();
                obj.FirstName = objuser.FirstName;
                obj.LastName = objuser.LastName;
                obj.Email = objuser.Email;
                obj.MaritalStatus = objuser.MaritalStatus;
                obj.StaffId = objuser.StaffId;
                obj.Designation = objuser.Designation;
                obj.IsActive = true;
                obj.IsProfile = false;
                db.Tbl_BankStaff.Add(obj);
                db.SaveChanges();
                return new ReturnMessage { Success = true, Message = "Registered Successfully!" };
            }
            return new ReturnMessage();
        }


        [Route("getAllActiveUsers")]
        [HttpGet]
        public Tbl_BankStaffVM[] AllUsers()
        {
            SCBDBEntities db = new SCBDBEntities();
            var res = from e in db.Tbl_BankStaff.Where(t => t.IsActive == true && t.IsProfile == true)
                      select new Tbl_BankStaffVM()
                      {
                          StaffId = e.StaffId,
                          Email = e.Email,
                          FirstName = e.FirstName,
                          LastName = e.LastName,
                          Designation = e.Designation,
                          MaritalStatus = e.MaritalStatus,
                          Id = e.Id
                      };
            return res.ToArray();
        }

        [Route("disableUser")]
        [HttpPost]
        public ReturnMessage DisableAppUser(string email)
        {
            SCBDBEntities db = new SCBDBEntities();
            if (!db.Tbl_Users.Any(e => e.Email == email))
            {
                return new ReturnMessage { Success = false, Message = "Email " + email + " does not exists!" };
            }
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
            if (cnnString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                cnnString = efBuilder.ProviderConnectionString;
            }
            SqlConnection conn = new SqlConnection(cnnString);
            SqlCommand com = new SqlCommand("sp_disableAppUser", conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@userEmail", email);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
            return new ReturnMessage { Success = false, Message = "Staff with Email " + email + " disabled!" };
        }

        public void ProfileUser(string email)
        {
            SCBDBEntities db = new SCBDBEntities();
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
            if (cnnString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                cnnString = efBuilder.ProviderConnectionString;
            }
            SqlConnection conn = new SqlConnection(cnnString);
            SqlCommand com = new SqlCommand("sp_profileAppUser", conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@userEmail", email);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
        }
    }

}

