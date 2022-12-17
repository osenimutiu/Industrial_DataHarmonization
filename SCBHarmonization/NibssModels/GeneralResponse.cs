using System;
using System.Text.Json.Serialization;

namespace SCBHarmonization.NibssModels
{
    public class GeneralResponse
    {
        public GeneralResponse()
        {
        }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
