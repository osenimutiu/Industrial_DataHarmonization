using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class tbl_ExDetailsViewModel
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string Vat { get; set; }
        public string Fee { get; set; }
        [Display(Name = "Transaction Id")]
        public string TransID { get; set; }
        [Display(Name = "Source Account Number")]
        public string SrcAcctNo { get; set; }
        [Display(Name = "Source Int. Code")]
        public string SrcInstCode { get; set; }
        [Display(Name = "Source Inst. BranchCode")]
        public string SrcInstBranchCode { get; set; }
        [Display(Name = "Source Inst. Type")]
        public string SrcInstTypes { get; set; }
        [Display(Name = "Source Inst. UniqueId")]
        public string SrcInstUniqueID { get; set; }
        [Display(Name = "Destination Account Number")]
        public string DestAcctNo { get; set; }
        [Display(Name = "Destination Inst. Code")]
        public string DestInstCode { get; set; }
        [Display(Name = "Destination Inst. Code")]
        public string DestInstBranchCode { get; set; }
        [Display(Name = "Destination Inst. Type")]
        public string DestInstTypes { get; set; }
        [Display(Name = "Destination Inst. UniqueId")]
        public string DestInstUniqueID { get; set; }
        [Display(Name = "Payment Type")]
        public string PaymentTypes { get; set; }
        [Display(Name = "Bank Income")]
        public string BankIncome { get; set; }
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public String TransDate { get; set; }
        public string PsspParty { get; set; }
        [Display(Name = "Account Type")]
        public string AccountTypes { get; set; }
        [Display(Name = "Account Class")]
        public string AccountClassTypes { get; set; }
        [Display(Name = "Account Designation")]
        public string AccountDesignationTypes { get; set; }
        public string CurrencyTypes { get; set; }
        public string ChannelTypes { get; set; }
        [Display(Name = "Transaction Type Code")]
        public string TransactionTypeCode { get; set; }
        [Display(Name = "Pep Designated Account")]
        public bool PepDesignatedAccount { get; set; }
        [Display(Name = "Cypher Security Levy Exempt")]
        public bool CypherSecurityLevyExempt { get; set; }
        [Display(Name = "Stamp Duty Exempt")]
        public bool StampDutyExempt { get; set; }
        public int TenantId { get; set; }
        [Display(Name = "Exception Type")]
        public string ExceptionType { get; set; }
        [Display(Name = "Additional Field")]
        public string AdditionalField { get; set; }
        public bool Inflow { get; set; }
    }
}