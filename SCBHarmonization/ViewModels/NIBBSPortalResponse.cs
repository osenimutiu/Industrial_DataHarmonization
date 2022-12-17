using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class NIBBSPortalResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }

    public class NIBBSPortalCheckList
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("exists")]
        public bool Exists { get; set; }
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }
    }

    //public class GetTransactionType
    //{
    //    [JsonPropertyName("amount")]
    //    public string Amount { get; set; }

    //    [JsonPropertyName("vat")]
    //    public string Vat { get; set; }

    //    [JsonPropertyName("fee")]
    //    public string Fee { get; set; }

    //    [JsonPropertyName("transID")]
    //    public string TransID { get; set; }

    //    [JsonPropertyName("srcAcctNo")]
    //    public string SrcAcctNo { get; set; }

    //    [JsonPropertyName("srcInstCode")]
    //    public string SrcInstCode { get; set; }

    //    [JsonPropertyName("srcInstBranchCode")]
    //    public string SrcInstBranchCode { get; set; }

    //    [JsonPropertyName("srcInstType")]
    //    public int? SrcInstType { get; set; }

    //    [JsonPropertyName("srcInstUniqueID")]
    //    public string SrcInstUniqueID { get; set; }

    //    [JsonPropertyName("destAcctNo")]
    //    public string DestAcctNo { get; set; }

    //    [JsonPropertyName("destInstCode")]
    //    public string DestInstCode { get; set; }

    //    [JsonPropertyName("destInstBranchCode")]
    //    public string DestInstBranchCode { get; set; }

    //    [JsonPropertyName("destInstType")]
    //    public int? DestInstType { get; set; }

    //    [JsonPropertyName("destInstUniqueID")]
    //    public string DestInstUniqueID { get; set; }

    //    [JsonPropertyName("bankIncome")]
    //    public string BankIncome { get; set; }

    //    [JsonPropertyName("transDate")]
    //    public string TransDate { get; set; }

    //    [JsonPropertyName("psspParty")]
    //    public string PsspParty { get; set; }

    //    [JsonPropertyName("accountType")]
    //    public int? AccountType { get; set; }

    //    [JsonPropertyName("accountClass")]
    //    public int? AccountClass { get; set; }

    //    [JsonPropertyName("accountDesignation")]
    //    public int? AccountDesignation { get; set; }

    //    [JsonPropertyName("currency")]
    //    public int? Currency { get; set; }

    //    [JsonPropertyName("paymentType")]
    //    public int? PaymentType { get; set; }

    //    [JsonPropertyName("channel")]
    //    public int? Channel { get; set; }

    //    [JsonPropertyName("transactionTypeCode")]
    //    public string TransactionTypeCode { get; set; }

    //    [JsonPropertyName("pepDesignatedAccount")]
    //    public bool PepDesignatedAccount { get; set; }

    //    [JsonPropertyName("cyberSecurityLevyExempt")]
    //    public bool CyberSecurityLevyExempt { get; set; }

    //    [JsonPropertyName("stampdutyExempt")]
    //    public bool StampdutyExempt { get; set; }

    //    public bool inflow { get; set; }

    //    [JsonPropertyName("emtl")]
    //    public bool Emtl { get; set; }

    //    [JsonPropertyName("receiverLocation")]
    //    public string ReceiverLocation { get; set; }
    //}

    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class GetTransactionTypeResult
    {
        public GetTransactionType[] result { get; set; }
    }
    public class GetTransactionType
    {
        public string code { get; set; }
        public string transactionGroupName { get; set; }
        public string name { get; set; }
    }

    public class SendSingleResult
    {
        public NIBBSPortalResponse result { get; set; }
    }

    public class SendBulkResult
    {
        public List<NIBBSPortalResponse> result { get; set; }
    }

    public class CheckExisting
    {
        public string TransId { get; set; }
    }

    public class FailedTransaction
    {
        public string TransId { get; set; }
    }

    public class IndividualTransaction
    {
        public int Id { get; set; }
    }

    public class CheckExistingResult
    {
        public NIBBSPortalCheckList result { get; set; }
    }
    public class CheckExistingListResult
    {
        public List<NIBBSPortalCheckList> result { get; set; }
    }

    public class CredentialsVM
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}