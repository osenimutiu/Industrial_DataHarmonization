using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Enter Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your UserName")]
        [Display(Name ="PSID")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Enter Your StaffName")]

        public string StaffName { get; set; }

        [Required(ErrorMessage = "Enter Your RoleName")]
        [Display(Name ="Roles")]
        public string RoleName { get; set; }
        public List<UserViewModel> ShowallUsers { get; set; }
    }

    public class DataAccessLayer
    {
        public int Deactivate(String Id)
        {
            SqlConnection con = null;
            int result;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand cmd = new SqlCommand("sp_GetAndDeactivate_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Email", null);
                cmd.Parameters.AddWithValue("@UserName", null);
                cmd.Parameters.AddWithValue("@StaffName", null);
                cmd.Parameters.AddWithValue("@Query", 3);
                con.Open();
                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch
            {
                return result = 0;
            }
            finally
            {
                con.Close();
            }
        }

        public List<UserViewModel> SelectAllUsers()
        {
            SqlConnection con = null;
            DataSet ds = null;
            List<UserViewModel> custlist = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand cmd = new SqlCommand("sp_GetAndDeactivate_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", null);
                cmd.Parameters.AddWithValue("@Email", null);
                cmd.Parameters.AddWithValue("@UserName", null);
                cmd.Parameters.AddWithValue("@StaffName", null);
                cmd.Parameters.AddWithValue("@Query", 4);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                custlist = new List<UserViewModel>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UserViewModel cobj = new UserViewModel();
                    //cobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerID"].ToString());
                    cobj.Id = ds.Tables[0].Rows[i]["Id"].ToString();
                    cobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                    cobj.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    cobj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    cobj.RoleName = ds.Tables[0].Rows[i]["Name"].ToString();
                    //cobj.Birthdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Birthdate"].ToString());
                    custlist.Add(cobj);
                }
                return custlist;
            }
            catch
            {
                return custlist;
            }
            finally
            {
                con.Close();
            }
        }

        public string SelectDatabyID(string id)
        {
            SqlConnection con = null;
            DataSet ds = null;
            UserViewModel cobj = null;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand("sp_GetAndDeactivate_Users", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Email", null);
            cmd.Parameters.AddWithValue("@UserName", null);
            cmd.Parameters.AddWithValue("@StaffName", null);
            cmd.Parameters.AddWithValue("@Query", 5);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cobj = new UserViewModel();
                cobj.Id = ds.Tables[0].Rows[i]["Id"].ToString();
                cobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                cobj.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                cobj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
            }
            return cobj.UserName;
          
        }
    }
}