using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
//using SCBDataHarmonization.Models;
using System.Web.Routing;
using SCBHarmonization.Models;

namespace SCBHarmonization.CustomFilter
{
    public class ExceptionHistory
    {
        public void UpdateExceptionHistory()
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
            cmd.CommandText = "sp_exception_history";
            cnn.Open();
            //object o = cmd.ExecuteScalar();
            cnn.Close();
        }
    }
    public class AppHelperMVC
    {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
        //public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
        //{
        //    public void OnAuthorization(AuthorizationContext filterContext)
        //    {
        //        SCBDBEntities db = new SCBDBEntities();
        //        string username = Convert.ToString(System.Web.HttpContext.Current.Session["Email"]);
        //        string role = Convert.ToString(System.Web.HttpContext.Current.Session["Role"]);
        //        string actionName = filterContext.ActionDescriptor.ActionName;
        //        string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //        string tag = controllerName + actionName;


        //        if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //        {
        //            return;
        //        }
        //        if (System.Web.HttpContext.Current.Session["Email"] == null)
        //        {
        //            filterContext.Result = new HttpUnauthorizedResult();
        //        }

        //        if (username != null && username != "")
        //        {
        //            bool isPermitted = false;

        //            var viewPermission = db.Tbl_RolePermission.Where(x => x.Role == role && x.Tag == tag).SingleOrDefault();
        //            if (viewPermission != null)
        //            {
        //                isPermitted = true;
        //            }
        //            if (isPermitted == false)
        //            {
        //                filterContext.Result = new RedirectToRouteResult(
        //                  new RouteValueDictionary
        //                    {
        //                     { "controller", "User" },
        //                     { "action", "AccessDenied" }
        //                    });
        //            }
        //        }
        //    }
        //}


}