﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class PreHarmonizationViewModel
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string Vat { get; set; }
        public string Fee { get; set; }
        public string TransID { get; set; }
        public string SrcAcctNo { get; set; }
        public string SrcInstCode { get; set; }
        public string SrcInstBranchCode { get; set; }
        public Nullable<int> SrcInstType { get; set; }
        public string SrcInstUniqueID { get; set; }
        public string DestAcctNo { get; set; }
        public string DestInstCode { get; set; }
        public string DestInstBranchCode { get; set; }
        public Nullable<int> DestInstType { get; set; }
        public string DestInstUniqueID { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public string BankIncome { get; set; }
        public string TransDate { get; set; }
        public string PsspParty { get; set; }
        public Nullable<int> AccountType { get; set; }
        public Nullable<int> AccountClass { get; set; }
        public Nullable<int> AccountDesignation { get; set; }
        public Nullable<int> Currency { get; set; }
        public Nullable<int> Channels { get; set; }
        public string TransactionTypeCode { get; set; }
        public bool PepDesignatedAccount { get; set; }
        public bool CypherSecurityLevyExempt { get; set; }
        public bool StampDutyExempt { get; set; }
        public string AdditionalField { get; set; }
        public bool Inflow { get; set; }
        public bool Emtl { get; set; }
        public string ReceiverLocation { get; set; }
        public string Status { get; set; }
    }
}