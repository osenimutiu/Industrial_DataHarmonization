using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.ViewModels
{
    public class tbl_ExViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string Vat { get; set; }
        [Required]
        public string Fee { get; set; }
        [Display(Name = "Transaction Id")]
        [Required]
        public string TransID { get; set; }
        [Display(Name = "Source Account Number")]
        [Required]
        public string SrcAcctNo { get; set; }
        [Display(Name = "Source Int. Code")]
        [Required]
        public string SrcInstCode { get; set; }
        [Display(Name = "Source Inst. BranchCode")]
        [Required]
        public string SrcInstBranchCode { get; set; }
        [Display(Name = "Source Inst. Type")]
        [Required]
        public int SrcInstType { get; set; }
        [Display(Name = "Source Inst. UniqueId")]
        [Required]
        public string SrcInstUniqueID { get; set; }
        [Display(Name = "Destination Account Number")]
        [Required]
        public string DestAcctNo { get; set; }
        [Display(Name = "Destination Inst. Code")]
        [Required]
        public string DestInstCode { get; set; }
        [Display(Name = "Destination Inst. Code")]
        [Required]
        public string DestInstBranchCode { get; set; }
        [Display(Name = "Destination Inst. Type")]
        [Required]
        public int DestInstType { get; set; }
        [Display(Name = "Destination Inst. UniqueId")]
        [Required]
        public string DestInstUniqueID { get; set; }
        [Display(Name = "Payment Type")]
        [Required]
        public int PaymentType { get; set; }
        [Display(Name = "Bank Income")]
        [Required]
        public string BankIncome { get; set; }
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public string TransDate { get; set; }
        [Required]
        public string PsspParty { get; set; }
        [Display(Name = "Account Type")]
        [Required]
        public int AccountType { get; set; }
        [Display(Name = "Account Class")]
        [Required]
        public int AccountClass { get; set; }
        [Display(Name = "Account Designation")]
        [Required]
        public int AccountDesignation { get; set; }
        [Required]
        public int Currency { get; set; }
        [Required]
        public int Channels { get; set; }
        [Display(Name = "Transaction Type Code")]
        [Required]
        public string TransactionTypeCode { get; set; }
        [Display(Name = "Pep Designated Account")]
        [Required]
        public bool PepDesignatedAccount { get; set; }
        [Display(Name = "Cypher Security Levy Exempt")]
        [Required]
        public bool CypherSecurityLevyExempt { get; set; }
        [Display(Name = "Stamp Duty Exempt")]
        [Required]
        public bool StampDutyExempt { get; set; }
        public int TenantId { get; set; }
        [Display(Name = "Exception Type")]
        public string ExceptionType { get; set; }
        [Display(Name = "Additional Field")]
        public string AdditionalField { get; set; }
        [Required]
        public bool Inflow { get; set; }
        public bool Emtl { get; set; }
        public string ReceiverLocation { get; set; }
        public List<SelectListItem> ListOfAccountClass { get; set; }
        public List<SelectListItem> ListOfAccountType { get; set; }
        public List<SelectListItem> ListOfAccountDesignation { get; set; }
        public List<SelectListItem> ListOfSrctInstType { get; set; }
        public List<SelectListItem> ListOfDestInstType { get; set; }
        public List<SelectListItem> ListOfPaymentType { get; set; }
        public List<SelectListItem> ListOfCurrency { get; set; }
        public List<SelectListItem> ListOfChannels { get; set; }

    }
}