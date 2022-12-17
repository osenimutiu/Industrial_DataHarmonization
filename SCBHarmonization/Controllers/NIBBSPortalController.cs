using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.NibssModels;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;  
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    public class NIBBSPortalController : Controller
    {
        
        string baseAddress = "https://api.nibss-plc.com.ng";
        private SCBDBEntities objDbEntities = new SCBDBEntities();
        private ApplicationSignInManager _signInManager;
        ApplicationDbContext context;
        AccountController acc = new AccountController();
        public NIBBSPortalController()
        {
            context = new ApplicationDbContext();
        }
        //public NIBBSPortalController()
        //{
           
        //    //UserManager = userManager;

        //}
        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        // GET: NIBBSPortal
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendSingleData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendSingleData(string id)
        {
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
           string message = String.Empty;
            Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
            string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
            string grant_type = "client_credentials";
            string scope = client_Id + "/.default";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
              {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
            }

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);

                httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));

                TransactionRequest[] dataList = (from s in objDbEntities.tbl_PreHarmonization
                                                     select new TransactionRequest()
                                                     {
                                                         amount = s.Amount,
                                                         vat = s.Vat,
                                                         fee = s.Fee,
                                                         transID = s.TransID,
                                                         srcAcctNo = s.SrcAcctNo,
                                                         srcInstCode = s.SrcInstCode,
                                                         srcInstBranchCode = s.SrcInstBranchCode,
                                                         srcInstType = s.SrcInstType,
                                                         srcInstUniqueID = s.SrcInstUniqueID,
                                                         destAcctNo = s.DestAcctNo,
                                                         destInstCode = s.DestInstCode,
                                                         destInstBranchCode = s.DestInstBranchCode,
                                                         destInstType = s.DestInstType,
                                                         destInstUniqueID = s.DestInstUniqueID,
                                                         bankIncome = s.BankIncome,
                                                         transDate = s.TransDate,
                                                         psspParty = s.PsspParty,
                                                         accountType = s.AccountType,
                                                         accountClass = s.AccountClass,
                                                         accountDesignation = s.AccountDesignation,
                                                         currency = s.Currency,
                                                         paymentType = s.PaymentType,
                                                         channel = s.Channels,
                                                         transactionTypeCode = s.TransactionTypeCode,
                                                         cyberSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                                         pepDesignatedAccount = s.PepDesignatedAccount,
                                                         stampdutyExempt = s.StampDutyExempt,
                                                         inflow = s.Inflow,
                                                         emtl = s.Emtl,
                                                         receiverLocation = s.ReceiverLocation
                                                     }).ToArray();
                

                foreach (TransactionRequest item in dataList)
                {
                    string stringDatab = JsonConvert.SerializeObject(item);
                    var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage list = httpClient.PostAsync("/idh/v2/sendData", contentDatab).Result;
                    //if (list.IsSuccessStatusCode)
                    //{
                       string corpResponse = list.Content.ReadAsStringAsync().Result;
                        postResp = JsonConvert.DeserializeObject<NIBBSPortalResponse>(corpResponse);
                    //postResp = dataResp.result;

                   var data = new Tbl_SendData
                        {
                            Status = postResp.Status,
                            Message = postResp.Message,
                            Description = postResp.Description,
                            Data = postResp.Data,
                            SentDate = DateTime.Now.ToString(),
                        };
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of  {postResp.Description}",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                     objDbEntities.Tbl_SendData.Add(data);
                    objDbEntities.SaveChanges();
                    //}
                }
                message = "Sent Successfully";
                ViewBag.Message = message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetTransactionData()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTransaction()
        {
            var dataList = objDbEntities.Tbl_SendData.OrderByDescending(x=> x.Id).Take(5000).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

      

        [HttpGet]
        public ActionResult SendBulkData()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SendBulkData(string bulk)
        {
            try
            {
                Token token = new Token();
                SendBulkResult dataResp = new SendBulkResult();
                NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
                string message = String.Empty;
                Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                string grant_type = "client_credentials";
                string scope = client_Id + "/.default";
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
                };
                    var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);

                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
                    if (cnnString.ToLower().StartsWith("metadata="))
                    {
                        System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                        cnnString = efBuilder.ProviderConnectionString;
                    }
                    SqlConnection cnn = new SqlConnection(cnnString);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_batchSending";
                    cnn.Open();
                    object o = cmd.ExecuteScalar();
                    List<int> dist = objDbEntities.tbl_PreHarmonization_Batch.Select(x => x.BatchCode).Distinct().ToList();
                    cnn.Close();

                    foreach (int i in dist)
                    {
                        TransactionRequest[] dataList = (from s in objDbEntities.tbl_PreHarmonization_Batch
                                                         where s.BatchCode == i 
                                                         select new TransactionRequest()
                                                         {
                                                             amount = s.Amount,
                                                             vat = s.Vat,
                                                             fee = s.Fee,
                                                             transID = s.TransID,
                                                             srcAcctNo = s.SrcAcctNo,
                                                             srcInstCode = s.SrcInstCode,
                                                             srcInstBranchCode = s.SrcInstBranchCode,
                                                             srcInstType = s.SrcInstType,
                                                             srcInstUniqueID = s.SrcInstUniqueID,
                                                             destAcctNo = s.DestAcctNo,
                                                             destInstCode = s.DestInstCode,
                                                             destInstBranchCode = s.DestInstBranchCode,
                                                             destInstType = s.DestInstType,
                                                             destInstUniqueID = s.DestInstUniqueID,
                                                             bankIncome = s.BankIncome,
                                                             transDate = s.TransDate,
                                                             psspParty = s.PsspParty,
                                                             accountType = s.AccountType,
                                                             accountClass = s.AccountClass,
                                                             accountDesignation = s.AccountDesignation,
                                                             currency = s.Currency,
                                                             paymentType = s.PaymentType,
                                                             channel = s.Channels,
                                                             transactionTypeCode = s.TransactionTypeCode,
                                                             cyberSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                                             pepDesignatedAccount = s.PepDesignatedAccount,
                                                             stampdutyExempt = s.StampDutyExempt,
                                                             inflow = s.Inflow,
                                                             emtl = s.Emtl,
                                                             receiverLocation = s.ReceiverLocation
                                                         }).ToArray();

                        string stringDatab = JsonConvert.SerializeObject(dataList);
                        var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage list = httpClient.PostAsync("/idh/v2/sendDataBulk", contentDatab).Result;
                        string corpResponse = list.Content.ReadAsStringAsync().Result;
                        postResp = JsonConvert.DeserializeObject<NIBBSPortalResponse[]>(corpResponse);
                        foreach (var item in postResp)
                        {
                            var data = new Tbl_SendData
                            {
                                Status = item.Status,
                                Message = item.Message,
                                Description = item.Description,
                                Data = item.Data,
                                SentDate = DateTime.Now.ToString(),
                            };
                            //var audit = new TBL_AUDIT()
                            //{
                            //    URL = Request.Url.AbsoluteUri,
                            //    UserName = User.Identity.Name,
                            //    DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                            //    IPADDRESS = SendEmail.GetLocalIpAddress(),
                            //    DEVICENAME = SendEmail.GetDeviceName(),
                            //    OSNAME = SendEmail.FriendlyName(),
                            //    DATETIME = DateTime.UtcNow,
                            //    UserId = User.Identity.GetUserId()
                            //};
                            //objDbEntities.TBL_AUDIT.Add(audit);
                            objDbEntities.Tbl_SendData.Add(data);
                            objDbEntities.SaveChanges();
                        }
                        //}
                    }
                    message = "Sent Successfully";
                    ViewBag.Message = message;
                }
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "SendBulk",
                    //InnerException = ex.InnerException.InnerException.Message,
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult SendBulkDatab(string bulk)
        {
            try
            {
                Token token = new Token();
                SendBulkResult dataResp = new SendBulkResult();
                NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
                string message = String.Empty;
                Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                string grant_type = "client_credentials";
                string scope = client_Id + "/.default";
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
                };
                    var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);

                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                    
                        TransactionRequest[] dataList = (from s in objDbEntities.tbl_PreHarmonization
                                                         select new TransactionRequest()
                                                         {
                                                             amount = s.Amount,
                                                             vat = s.Vat,
                                                             fee = s.Fee,
                                                             transID = s.TransID,
                                                             srcAcctNo = s.SrcAcctNo,
                                                             srcInstCode = s.SrcInstCode,
                                                             srcInstBranchCode = s.SrcInstBranchCode,
                                                             srcInstType = s.SrcInstType,
                                                             srcInstUniqueID = s.SrcInstUniqueID,
                                                             destAcctNo = s.DestAcctNo,
                                                             destInstCode = s.DestInstCode,
                                                             destInstBranchCode = s.DestInstBranchCode,
                                                             destInstType = s.DestInstType,
                                                             destInstUniqueID = s.DestInstUniqueID,
                                                             bankIncome = s.BankIncome,
                                                             transDate = s.TransDate,
                                                             psspParty = s.PsspParty,
                                                             accountType = s.AccountType,
                                                             accountClass = s.AccountClass,
                                                             accountDesignation = s.AccountDesignation,
                                                             currency = s.Currency,
                                                             paymentType = s.PaymentType,
                                                             channel = s.Channels,
                                                             transactionTypeCode = s.TransactionTypeCode,
                                                             cyberSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                                             pepDesignatedAccount = s.PepDesignatedAccount,
                                                             stampdutyExempt = s.StampDutyExempt,
                                                             inflow = s.Inflow,
                                                             emtl = s.Emtl,
                                                             receiverLocation = s.ReceiverLocation
                                                         }).Take(500).ToArray();

                        string stringDatab = JsonConvert.SerializeObject(dataList);
                        var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage list = httpClient.PostAsync("/idh/v2/sendDataBulk", contentDatab).Result;
                        string corpResponse = list.Content.ReadAsStringAsync().Result;
                        postResp = JsonConvert.DeserializeObject<NIBBSPortalResponse[]>(corpResponse);
                        foreach (var item in postResp)
                        {
                            var data = new Tbl_SendData
                            {
                                Status = item.Status,
                                Message = item.Message,
                                Description = item.Description,
                                Data = item.Data,
                                SentDate = DateTime.Now.ToString(),
                            };
                            var audit = new TBL_AUDIT()
                            {
                                URL = Request.Url.AbsoluteUri,
                                UserName = User.Identity.Name,
                                DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                                IPADDRESS = SendEmail.GetLocalIpAddress(),
                                DEVICENAME = SendEmail.GetDeviceName(),
                                OSNAME = SendEmail.FriendlyName(),
                                DATETIME = DateTime.UtcNow,
                                UserId = User.Identity.GetUserId()
                            };
                            objDbEntities.TBL_AUDIT.Add(audit);
                            objDbEntities.Tbl_SendData.Add(data);
                            objDbEntities.SaveChanges();
                        }
                        //}
                        message = "Sent Successfully";
                        ViewBag.Message = message;
                    
                }
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "SendBulk",
                    //InnerException = ex.InnerException.InnerException.Message,
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }

            return RedirectToAction("SendBulkData");
        }


        [HttpGet]
        public ActionResult CheckExisting()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CheckExisting(CheckExisting transId)
        {
            NIBBSPortalCheckList postResp = new NIBBSPortalCheckList();
            try
            {
                Token token = new Token();
                CheckExistingResult dataResp = new CheckExistingResult();
                string message = String.Empty;
                string[] obj = new string[] { transId.TransId };
                //obj[0] = transId.TransId;
                Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                string grant_type = "client_credentials";
                string scope = client_Id + "/.default";
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
              {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                    var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }


                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                    string stringDatab = JsonConvert.SerializeObject(obj);
                    var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage list = httpClient.PostAsync("/idh/v2/checkExisting", contentDatab).Result;
                    //if (list.IsSuccessStatusCode)
                    //{
                    string corpResponse = list.Content.ReadAsStringAsync().Result;
                    postResp = JsonConvert.DeserializeObject<NIBBSPortalCheckList>(corpResponse);
                    //postResp = dataResp.result;
                    var data = new Tbl_CheckExisting
                    {
                        Status = postResp.Status,
                        Message = postResp.Message,
                        Description = postResp.Description,
                        TransactionId = postResp.TransactionId,
                        Exists = postResp.Exists,
                        SendDate = DateTime.Now.ToString(),
                    };
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"Transaction of transactionId '{postResp.TransactionId}' validated",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                    objDbEntities.Tbl_CheckExisting.Add(data);
                    objDbEntities.SaveChanges();
                    //}
                }
                //return Json(postResp.Description, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "CheckExisting",
                    //InnerException = ex.InnerException.InnerException.Message,
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }
            return Json(postResp.Description, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CheckExistingList(CheckExisting transId)
        {
            string message = String.Empty;
            try
            {
                Token token = new Token();
                CheckExistingListResult dataResp = new CheckExistingListResult();
                NIBBSPortalCheckList[] postResp = new NIBBSPortalCheckList[10000];
                Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                string grant_type = "client_credentials";
                string scope = client_Id + "/.default";
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
              {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                    var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                    string[] transIdList = objDbEntities.tbl_PreHarmonization.Select(x => x.TransID).ToArray();

                    string stringDatab = JsonConvert.SerializeObject(transIdList);
                    //string stringDatab = JsonConvert.SerializeObject(transIdArray);
                    var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage list = httpClient.PostAsync("/idh/v2/CheckExistingList", contentDatab).Result;
                    //if (list.IsSuccessStatusCode)
                    //{
                    string corpResponse = list.Content.ReadAsStringAsync().Result;
                    postResp = JsonConvert.DeserializeObject<NIBBSPortalCheckList[]>(corpResponse);
                    //postResp = dataResp.result;
                    foreach (NIBBSPortalCheckList item in postResp)
                    {
                        var data = new Tbl_CheckExisting
                        {
                            Status = item.Status,
                            Message = item.Message,
                            Description = item.Description,
                            TransactionId = item.TransactionId,
                            Exists = item.Exists,
                            SendDate = DateTime.Now.ToString(),
                        };
                        var audit = new TBL_AUDIT()
                        {
                            URL = Request.Url.AbsoluteUri,
                            UserName = User.Identity.Name,
                            DETAIL = $"Transaction of transactionId '{item.TransactionId}' validated",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = User.Identity.GetUserId()
                        };
                        objDbEntities.TBL_AUDIT.Add(audit);
                        objDbEntities.Tbl_CheckExisting.Add(data);
                        objDbEntities.SaveChanges();
                    }
                    message = "Checked Successfully";
                    //}
                }
                //return RedirectToAction("CheckExisting");
                //return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "CheckExistingList",
                    //InnerException = ex.InnerException.InnerException.Message,
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckExistingList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetExistingListResult()
        {
            var dataList = objDbEntities.Tbl_CheckExisting.OrderByDescending(x => x.Id).Take(5000).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTransactionType()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTransactionTypeB()
        {
            Token token = new Token();
            GetTransactionTypeResult itemList = new GetTransactionTypeResult();
            GetTransactionType[] translList = new GetTransactionType[50000];
            string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
            string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
            Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
            //string scope = cred.ClientId + "/.default";
            string scope = client_Id + "/.default";
            using (var client = new HttpClient())
            {
              var form = new Dictionary<string, string>
              {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                    HttpResponseMessage response = httpClient.GetAsync("/idh/v2/getTransactionTypes").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string message = response.Content.ReadAsStringAsync().Result;
                        translList = JsonConvert.DeserializeObject<GetTransactionType[]>(message);
                    }
                }
                return Json(translList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Credential()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("System Admin"))
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public JsonResult Credential(CredentialsVM cred)
        {
            string mess = "";
            int LastCredentialId = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            Tbl_Credential LastCredential = objDbEntities.Tbl_Credential.Find(LastCredentialId);
            try
            {
                if(cred.ClientId == null || cred.ClientId == "")
                {
                    LastCredential.ClientSecret = cred.ClientSecret;
                }
                else
                {
                    LastCredential.ClientId = cred.ClientId;
                    LastCredential.ClientSecret = cred.ClientSecret;
                }
                //Tbl_Credential data = new Tbl_Credential
                //{
                //    ClientId = cred.ClientId,
                //    ClientSecret = cred.ClientSecret,
                //    Scope = cred.ClientId + "/.default"
                //};
                var audit = new TBL_AUDIT()
                {
                    URL = Request.Url.AbsoluteUri,
                    UserName = User.Identity.Name,
                    DETAIL = $"Credentials updated by '{ User.Identity.Name}'",
                    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    DEVICENAME = SendEmail.GetDeviceName(),
                    OSNAME = SendEmail.FriendlyName(),
                    DATETIME = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId()
                };
                objDbEntities.TBL_AUDIT.Add(audit);
                //objDbEntities.Tbl_Credential.Add(data);
                objDbEntities.SaveChanges();
                 mess = "Credentials Saved Successfully";
                //return Json(mess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "Credential",
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }
            return Json(mess, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnsendData()
        {
            SCBDBEntities ex = new SCBDBEntities();
            tbl_ExViewModel objectRoomViewModel = new tbl_ExViewModel();
            objectRoomViewModel.ListOfAccountClass = (from obj in ex.tbl_AccountClass
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.AccountClassTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfAccountType = (from obj in ex.tbl_AccountType
                                                     select new SelectListItem()
                                                     {
                                                         Text = obj.AccountTypes,
                                                         Value = obj.Id.ToString()


                                                     }).ToList();
            objectRoomViewModel.ListOfAccountDesignation = (from obj in ex.tbl_AccountDesignation
                                                            select new SelectListItem()
                                                            {
                                                                Text = obj.AccountDesignationTypes,
                                                                Value = obj.Id.ToString()


                                                            }).ToList();
            objectRoomViewModel.ListOfSrctInstType = (from obj in ex.tbl_SrcInsType
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.SrcInstTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfDestInstType = (from obj in ex.tbl_DestInstType
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.DestInstTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfPaymentType = (from obj in ex.tbl_PaymentTypes
                                                     select new SelectListItem()
                                                     {
                                                         Text = obj.PaymentTypes,
                                                         Value = obj.Id.ToString()


                                                     }).ToList();
            objectRoomViewModel.ListOfCurrency = (from obj in ex.tbl_Currency
                                                  select new SelectListItem()
                                                  {
                                                      Text = obj.CurrencyTypes,
                                                      Value = obj.Id.ToString()


                                                  }).ToList();
            objectRoomViewModel.ListOfChannels = (from obj in ex.tbl_Channel
                                                  select new SelectListItem()
                                                  {
                                                      Text = obj.ChannelTypes,
                                                      Value = obj.Id.ToString()


                                                  }).ToList();
            return View(objectRoomViewModel);
        }
        
        [HttpGet]
        public JsonResult GetUnsendData(string transId)
        {
            var data = objDbEntities.tbl_PreHarmonization.FirstOrDefault(x=>x.TransID.Equals(transId, StringComparison.OrdinalIgnoreCase));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUnsendData(tbl_PreHarmonization obj)
        {
            SCBDBEntities ent = new SCBDBEntities();
            string message = String.Empty;
            var res = ent.tbl_PreHarmonization.FirstOrDefault(x => x.TransID == obj.TransID);
            res.Amount = obj.Amount;
            res.Vat = obj.Vat;
            res.Fee = obj.Fee;
            res.TransID = obj.TransID;
            res.SrcAcctNo = obj.SrcAcctNo;
            res.SrcInstCode = obj.SrcInstCode;
            res.SrcInstBranchCode = obj.SrcInstBranchCode;
            res.SrcInstType = obj.SrcInstType;
            res.SrcInstUniqueID = obj.SrcInstUniqueID;
            res.DestAcctNo = obj.DestAcctNo;
            res.DestInstCode = obj.DestInstCode;
            res.DestInstBranchCode = obj.DestInstBranchCode;
            res.DestInstType = obj.DestInstType;
            res.DestInstUniqueID = obj.DestInstUniqueID;
            res.BankIncome = obj.BankIncome;
            res.TransDate = obj.TransDate;
            res.PsspParty = obj.PsspParty;
            res.AccountType = obj.AccountType;
            res.AccountClass = obj.AccountClass;
            res.AccountDesignation = obj.AccountDesignation;
            res.Currency = obj.Currency;
            res.PaymentType = obj.PaymentType;
            res.Channels = obj.Channels;
            res.TransactionTypeCode = obj.TransactionTypeCode;
            res.CypherSecurityLevyExempt = obj.CypherSecurityLevyExempt;
            res.PepDesignatedAccount = obj.PepDesignatedAccount;
            res.StampDutyExempt = obj.StampDutyExempt;
            res.Inflow = obj.Inflow;
            res.Emtl = obj.Emtl;
            res.ReceiverLocation = obj.ReceiverLocation;
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"Transaction of transactionId '{res.TransID}' updated",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            ent.TBL_AUDIT.Add(audit);
            ent.SaveChanges();
            message = "Updated Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnsentTransactions()
        {
            return View();
        }

        [HttpGet]
        public JsonResult UnsentTransactionResult()
        {
            SCBDBEntities ent = new SCBDBEntities();
            var dataList = ent.Tbl_SendData.Where(x => !x.Status.Equals("00", StringComparison.OrdinalIgnoreCase)).OrderByDescending(y=>y.Id).Take(5000).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SendFailedTransaction()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SendFailedTransaction(FailedTransaction transId)
        {
            string[] obj = new string[] { transId.TransId };
            //obj[0] = transId.TransId;
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            string message = String.Empty;
            bool dataSent = objDbEntities.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(transId.TransId));
            if (dataSent)
            {
                string isExist = "There's already an existing record with transactionId " + transId.TransId + " at NIBSS endpoint";
                return Json(isExist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                string grant_type = "client_credentials";
                string scope = client_Id + "/.default";
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
              {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                    var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }


                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));


                    TransactionRequest dataEntity = (from s in objDbEntities.tbl_PreHarmonization.Where(y=>y.TransID == transId.TransId)
                                                     select new TransactionRequest()
                                                     {
                                                         amount = s.Amount,
                                                         vat = s.Vat,
                                                         fee = s.Fee,
                                                         transID = s.TransID,
                                                         srcAcctNo = s.SrcAcctNo,
                                                         srcInstCode = s.SrcInstCode,
                                                         srcInstBranchCode = s.SrcInstBranchCode,
                                                         srcInstType = s.SrcInstType,
                                                         srcInstUniqueID = s.SrcInstUniqueID,
                                                         destAcctNo = s.DestAcctNo,
                                                         destInstCode = s.DestInstCode,
                                                         destInstBranchCode = s.DestInstBranchCode,
                                                         destInstType = s.DestInstType,
                                                         destInstUniqueID = s.DestInstUniqueID,
                                                         bankIncome = s.BankIncome,
                                                         transDate = s.TransDate,
                                                         psspParty = s.PsspParty,
                                                         accountType = s.AccountType,
                                                         accountClass = s.AccountClass,
                                                         accountDesignation = s.AccountDesignation,
                                                         currency = s.Currency,
                                                         paymentType = s.PaymentType,
                                                         channel = s.Channels,
                                                         transactionTypeCode = s.TransactionTypeCode,
                                                         cyberSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                                         pepDesignatedAccount = s.PepDesignatedAccount,
                                                         stampdutyExempt = s.StampDutyExempt,
                                                         inflow = s.Inflow,
                                                         emtl = s.Emtl,
                                                         receiverLocation = s.ReceiverLocation
                                                     }).FirstOrDefault();
                    string stringDatab = JsonConvert.SerializeObject(dataEntity);
                    var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage list = httpClient.PostAsync("/idh/v2/sendData", contentDatab).Result;
                    //if (list.IsSuccessStatusCode)
                    //{
                    string corpResponse = list.Content.ReadAsStringAsync().Result;
                    postResp = JsonConvert.DeserializeObject<NIBBSPortalResponse>(corpResponse);
                    //postResp = dataResp.result;
                    var data = new Tbl_SendData
                    {
                        Status = postResp.Status,
                        Message = postResp.Message,
                        Description = postResp.Description,
                        Data = postResp.Data,
                        SentDate = DateTime.Now.ToString(),
                    };
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"Transaction of transactionId '{postResp.Data}' has response {postResp.Description}",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                    objDbEntities.Tbl_SendData.Add(data);
                    objDbEntities.SaveChanges();
                    //}
                }
                return Json(postResp.Description, JsonRequestBehavior.AllowGet);
            }

            
        }

       

        [HttpPost]
        public JsonResult SendIndTransaction(IndividualTransaction Id)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            try
            {
                Token token = new Token();
                SendSingleResult dataResp = new SendSingleResult();
                
                string message = String.Empty;
                string transId = objDbEntities.tbl_PreHarmonization.FirstOrDefault(x => x.Id == Id.Id).TransID;
                tbl_PreHarmonization objToSend = objDbEntities.tbl_PreHarmonization.FirstOrDefault(x => x.TransID == transId);
                bool dataSent = objDbEntities.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
                if (dataSent)
                {
                    string isExist = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                    return Json(isExist, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Tbl_Credential cred = objDbEntities.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                    string client_Id = "27ea5cd0-efd2-4173-8c3c-e47760ce5a9b";
                    string client_secret = "gwC8Q~oeghFXLLmjoAu2cqbN7eLLbHZBNRrKSc.F";
                    string grant_type = "client_credentials";
                    string scope = client_Id + "/.default";
                    using (var client = new HttpClient())
                    {
                        var form = new Dictionary<string, string>
               {
                   {"grant_type", grant_type},
                   {"client_secret", cred.ClientSecret},
                   {"client_Id", cred.ClientId},
                   {"scope", cred.Scope},
               };
                        var tokenResponse = client.PostAsync(baseAddress + "/reset", new FormUrlEncodedContent(form)).Result;
                        token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                    }


                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(baseAddress);
                        httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                        //objToSend

                        TransactionRequest dataEntity = new TransactionRequest
                        {
                            amount = objToSend.Amount,
                            vat = objToSend.Vat,
                            fee = objToSend.Fee,
                            transID = objToSend.TransID,
                            srcAcctNo = objToSend.SrcAcctNo,
                            srcInstCode = objToSend.SrcInstCode,
                            srcInstBranchCode = objToSend.SrcInstBranchCode,
                            srcInstType = objToSend.SrcInstType,
                            srcInstUniqueID = objToSend.SrcInstUniqueID,
                            destAcctNo = objToSend.DestAcctNo,
                            destInstCode = objToSend.DestInstCode,
                            destInstBranchCode = objToSend.DestInstBranchCode,
                            destInstType = objToSend.DestInstType,
                            destInstUniqueID = objToSend.DestInstUniqueID,
                            bankIncome = objToSend.BankIncome,
                            transDate = objToSend.TransDate,
                            psspParty = objToSend.PsspParty,
                            accountType = objToSend.AccountType,
                            accountClass = objToSend.AccountClass,
                            accountDesignation = objToSend.AccountDesignation,
                            currency = objToSend.Currency,
                            paymentType = objToSend.PaymentType,
                            channel = objToSend.Channels,
                            transactionTypeCode = objToSend.TransactionTypeCode,
                            cyberSecurityLevyExempt = objToSend.CypherSecurityLevyExempt,
                            pepDesignatedAccount = objToSend.PepDesignatedAccount,
                            stampdutyExempt = objToSend.StampDutyExempt,
                            inflow = objToSend.Inflow,
                            emtl = objToSend.Emtl,
                            receiverLocation = objToSend.ReceiverLocation
                        };
                        string stringDatab = JsonConvert.SerializeObject(dataEntity);
                        var contentDatab = new StringContent(stringDatab, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage list = httpClient.PostAsync("/idh/v2/sendData", contentDatab).Result;
                        //if (list.IsSuccessStatusCode)
                        //{

                        string corpResponse = list.Content.ReadAsStringAsync().Result;
                        postResp = JsonConvert.DeserializeObject<NIBBSPortalResponse>(corpResponse);
                        //postResp = dataResp.result;
                        var data = new Tbl_SendData
                        {
                            Status = postResp.Status,
                            Message = postResp.Message,
                            Description = postResp.Description,
                            Data = postResp.Data,
                            SentDate = DateTime.Now.ToString()
                        };
                        if (postResp.Status == "00")
                        {
                            objToSend.Status = "Sent";
                        }
                        else
                        {
                            objToSend.Status = "Pending";
                        }
                        var audit = new TBL_AUDIT()
                        {
                            URL = Request.Url.AbsoluteUri,
                            UserName = User.Identity.Name,
                            DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = User.Identity.GetUserId()
                        };
                        objDbEntities.TBL_AUDIT.Add(audit);
                        objDbEntities.Tbl_SendData.Add(data);
                        objDbEntities.SaveChanges();
                    }
                    //return Json(postResp.Description, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "NIBBSPortal",
                    ActionName = "SendIndTransaction",
                    //InnerException = ex.InnerException.InnerException.Message,
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objDbEntities.ExceptionLoggers.Add(logger);
                objDbEntities.SaveChanges();
            }
            return Json(postResp.Description, JsonRequestBehavior.AllowGet);
        }

    }
}