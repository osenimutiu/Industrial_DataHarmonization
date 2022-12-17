using System;
using System.Text.Json.Serialization;

namespace SCBHarmonization.NibssModels
{



    public class TransactionType
    {
        public TransactionType()
        {
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("transactionGroupName")]
        public string TransactionGroupName { get; set; }
    }
}
