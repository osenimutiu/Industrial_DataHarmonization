using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class tbl_TransactionDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Successful Trans. Count")]
        [Required]
        public int SucTransCount { get; set; }
        [Display(Name = "Failled Trans. Count")]
        [Required]
        public int FailTransCount { get; set; }
        [Display(Name = "Transaction Date")]
        [Required]
        public System.DateTime TransDate { get; set; }
    }
}