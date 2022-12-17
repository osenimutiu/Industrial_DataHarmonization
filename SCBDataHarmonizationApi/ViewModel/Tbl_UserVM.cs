using SCBDataHarmonizationApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static SCBDataHarmonizationApi.Helper.AppHelper;

namespace SCBDataHarmonizationApi.ViewModel
{
      public class Tbl_UserVM
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Tbl_BankStaffVM
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        //public Nullable<bool> IsActive { get; set; }
    }

    public class Disable
    {
        

    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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
    }
   
}