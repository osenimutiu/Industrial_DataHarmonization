using System;
using System.Text.Json.Serialization;

namespace SCBHarmonization.NibssModels
{
    public class Transaction
    {
        public Transaction()
        {
        }
    }

    public class TransactionRequest
    {
        [JsonPropertyName("amount")]
        public string amount { get; set; }

        [JsonPropertyName("vat")]
        public string vat { get; set; }

        [JsonPropertyName("fee")]
        public string fee { get; set; }

        [JsonPropertyName("transID")]
        public string transID { get; set; }

        [JsonPropertyName("srcAcctNo")]
        public string srcAcctNo { get; set; }

        [JsonPropertyName("srcInstCode")]
        public string srcInstCode { get; set; }

        [JsonPropertyName("srcInstBranchCode")]
        public string srcInstBranchCode { get; set; }

        [JsonPropertyName("srcInstType")]
        public int? srcInstType { get; set; }

        [JsonPropertyName("srcInstUniqueID")]
        public string srcInstUniqueID { get; set; }

        [JsonPropertyName("destAcctNo")]
        public string destAcctNo { get; set; }

        [JsonPropertyName("destInstCode")]
        public string destInstCode { get; set; }

        [JsonPropertyName("destInstBranchCode")]
        public string destInstBranchCode { get; set; }

        [JsonPropertyName("destInstType")]
        public int? destInstType { get; set; }

        [JsonPropertyName("destInstUniqueID")]
        public string destInstUniqueID { get; set; }

        [JsonPropertyName("bankIncome")]
        public string bankIncome { get; set; }

        [JsonPropertyName("transDate")]
        public string transDate { get; set; }

        [JsonPropertyName("psspParty")]
        public string psspParty { get; set; }

        [JsonPropertyName("accountType")]
        public int? accountType { get; set; }

        [JsonPropertyName("accountClass")]
        public int? accountClass { get; set; }

        [JsonPropertyName("accountDesignation")]
        public int? accountDesignation { get; set; }

        [JsonPropertyName("currency")]
        public int? currency { get; set; }

        [JsonPropertyName("paymentType")]
        public int? paymentType { get; set; }

        [JsonPropertyName("channel")]
        public int? channel { get; set; }

        [JsonPropertyName("transactionTypeCode")]
        public string transactionTypeCode { get; set; }

        [JsonPropertyName("pepDesignatedAccount")]
        public bool pepDesignatedAccount { get; set; }

        [JsonPropertyName("cyberSecurityLevyExempt")]
        public bool cyberSecurityLevyExempt { get; set; }

        [JsonPropertyName("stampdutyExempt")]
        public bool stampdutyExempt { get; set; }

        public bool inflow { get; set; }

        [JsonPropertyName("emtl")]
        public bool emtl { get; set; }

        [JsonPropertyName("receiverLocation")]
        public string receiverLocation { get; set; }
       public string additionalFields { get; set; }

    }




    public class TransactionResponse: GeneralResponse
    {


        [JsonPropertyName("data")]
        public string Data { get; set; }
    }


}
