using System;
using System.Text.Json.Serialization;

namespace SCBHarmonization.NibssModels
{
    public class ResetModel
    {
        public ResetModel()
        {
        }
    }



    public class ResetModelRequest
    {
        public ResetModelRequest()
        {
        }
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }


    public class ResetModelResponse: GeneralResponse
    {
        public ResetModelResponse()
        {
        }
        public string IV { get; set; }
        public string Key { get; set; }
    }
}
