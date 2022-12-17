using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SCBHarmonization.Models;
using SCBHarmonization.NibssModels;

namespace SCBHarmonization.NibssClass
{

    public interface IDataAggregationInterface
    {
        Task<GeneralResponse> PingAsync();
        Task<ResetModelResponse> ResetCredentialAsync(ResetModelRequest resetModelRequest);
        Task<List<TransactionType>> GetAllTransactionTypeAsync();
        Task<TransactionResponse> SendTransactionDataAsync(TransactionRequest transactionRequest, (string iv, string key, string username) userDetails);
        Task<TransactionResponse> SendTransactionDataListAsync(List<TransactionRequest> transactionRequest, (string iv, string key, string username) userDetails);

    }


    public class DataAggregationInplementation : IDataAggregationInterface
    {

        private readonly IHttpClientFactory _httpClientFactory;


        public DataAggregationInplementation(IHttpClientFactory clientFactory)
        {
            
            _httpClientFactory = clientFactory;
            

        }


        public async Task<GeneralResponse> PingAsync()
        {

            var client = _httpClientFactory.CreateClient("DataAggregation");

            var request = new HttpRequestMessage(HttpMethod.Get, "ping");

            var response = await client.SendAsync(request);


                var resetResponseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GeneralResponse>(resetResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            
        }

        public async Task<ResetModelResponse> ResetCredentialAsync(ResetModelRequest resetModelRequest)
        {
            ResetModelResponse resetModelResponse = new ResetModelResponse { Description = "", Message = "Not Processed", Status = "", IV = "", Key = "" };

            var client = _httpClientFactory.CreateClient("DataAggregation");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/reset");

            var resetRequestString =JsonSerializer.Serialize(resetModelRequest, typeof(ResetModelRequest), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var stringContent = new StringContent(resetRequestString, Encoding.UTF8, "application/json");

            request.Content = stringContent;


            var response = await client.SendAsync(request);

           
            if (response.IsSuccessStatusCode)
            {
                var resetResponseString =  await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResetModelResponse>(resetResponseString,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return resetModelResponse;
        }



        public async Task<List<TransactionType>> GetAllTransactionTypeAsync()
        {
            List<TransactionType> AllTransactionTypes = new List<TransactionType>();
        
            var client = _httpClientFactory.CreateClient("DataAggregation");

            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/getTransactionTypes");

            var response = await client.SendAsync(request);

     

            if (response.IsSuccessStatusCode)
            {
                var resetResponseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TransactionType>>(resetResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return AllTransactionTypes;
        }

        public async Task<TransactionResponse> SendTransactionDataAsync(TransactionRequest transactionRequest, (string iv, string key, string username) userDetails)
        {
        

            var client = _httpClientFactory.CreateClient("DataAggregation");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/send");

            var sendDataRequestString = JsonSerializer.Serialize(transactionRequest, typeof(TransactionRequest), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            AES crypto = new AES(userDetails.key, userDetails.iv);

            var sendDataRequestStringCipher =  crypto.Encrypt(sendDataRequestString);

            var stringContent = new StringContent(sendDataRequestStringCipher, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var headers = GenerateNIBBSBasicHeaders(userDetails.username, userDetails.key, userDetails.iv);

            client.DefaultRequestHeaders.TryAddWithoutValidation("USERNAME", headers.USERNAME);
            client.DefaultRequestHeaders.TryAddWithoutValidation("SIGNATURE", headers.SIGNATURE);
            client.DefaultRequestHeaders.TryAddWithoutValidation("SIGNATURE_METH", headers.SIGNATURE_METH);

            var response  = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {

                var Encrypted = await response.Content.ReadAsStringAsync();
                var clearDataSend = crypto.Decrypt(Encrypted);

                return JsonSerializer.Deserialize<TransactionResponse>(clearDataSend, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new TransactionResponse {  Status = "", Description="NOT PROCESSED"};


        }



        public SecurityHeaders GenerateNIBBSBasicHeaders(string Username, string Key, string IV)
        {


            var FullCurrentTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            var DayCurrentTimeStamp = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            SecurityHeaders header = new SecurityHeaders();

            header.USERNAME = ToBASE64String(Username);
            header.SIGNATURE_METH = "SHA256";
            header.SIGNATURE = CryptoUtilities.SHA.GenerateSHA256String(string.Concat(Username, DayCurrentTimeStamp, Key,IV));

            return header;
        }
        private string ToBASE64String(string ConcatenatedCredentials)
        {
            var ByteCredentials = Encoding.UTF8.GetBytes(ConcatenatedCredentials);
            var BASE64Credentials = Convert.ToBase64String(ByteCredentials);
            return BASE64Credentials;
        }


        public async Task<TransactionResponse> SendTransactionDataListAsync(List<TransactionRequest> transactionRequest, (string iv, string key, string username) userDetails)
        {
            var client = _httpClientFactory.CreateClient("DataAggregation");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/sendDataBulk");

            var sendDataRequestString = JsonSerializer.Serialize(transactionRequest, typeof(tbl_PreHarmonization), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            AES crypto = new AES(userDetails.key, userDetails.iv);

            var sendDataRequestStringCipher = crypto.Encrypt(sendDataRequestString);

            var stringContent = new StringContent(sendDataRequestStringCipher, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var headers = GenerateNIBBSBasicHeaders(userDetails.username, userDetails.key, userDetails.iv);

            client.DefaultRequestHeaders.TryAddWithoutValidation("USERNAME", headers.USERNAME);
            client.DefaultRequestHeaders.TryAddWithoutValidation("SIGNATURE", headers.SIGNATURE);
            client.DefaultRequestHeaders.TryAddWithoutValidation("SIGNATURE_METH", headers.SIGNATURE_METH);

            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {

                var Encrypted = await response.Content.ReadAsStringAsync();
                var clearDataSend = crypto.Decrypt(Encrypted);

                return JsonSerializer.Deserialize<TransactionResponse>(clearDataSend, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new TransactionResponse { Status = "", Description = "NOT PROCESSED" };

        }

    }
}
