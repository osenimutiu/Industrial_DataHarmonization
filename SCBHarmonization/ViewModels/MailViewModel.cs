using SCBHarmonization.CustomFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.ViewModels
{
    public class MailViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        //[Required(ErrorMessage = "Subject required")]
        public string Subject { get; set; }
        //[Required(ErrorMessage = "Body required")]
        public string Body { get; set; }
        public string AdditionalComment { get; set; }
    }
    public class Tbl_UserVM
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
        //public List<SelectListItem> ListOfRoles{ get; set; }
    }

    public class LoginVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ResetPassVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        //[StringLength(10, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
        
    }
}