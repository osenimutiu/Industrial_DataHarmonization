//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCBHarmonization.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_CheckExisting
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public bool Exists { get; set; }
        public string TransactionId { get; set; }
        public string SendDate { get; set; }
    }
}