using SCBHarmonization.Models;
using SCBHarmonization.NibssModels;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace SCBHarmonization.Helper
{
    public class RoleBasedTransaction
    {
        SCBDBEntities db = new SCBDBEntities();
        string baseAddress = "https://api.nibss-plc.com.ng";
        public IQueryable<PreHarmonizationViewModel> GetEbbsCheckerTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_EbbsChecker  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_EbbsChecker /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });
            
            return transactionItem;
        }

        public IQueryable<PreHarmonizationViewModel> GetEbbsMakerTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_EbbsMaker  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_EbbsMaker /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });

            return transactionItem;
        }

        public IQueryable<PreHarmonizationViewModel> GetOpalCheckerTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_OpalChecker  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_OpalChecker /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });

            return transactionItem;
        }

        public IQueryable<PreHarmonizationViewModel> GetOpalMakerTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_OpalMaker  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_OpalMaker/*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });

            return transactionItem;
        }

        public IQueryable<PreHarmonizationViewModel> GetEbbsTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_ForEbbs  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_ForEbbs /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });

            return transactionItem;
        }

        public IQueryable<PreHarmonizationViewModel> GetOpalTransaction()
        {
            db.Database.ExecuteSqlCommand("update tbl_PreHarmonization_ForOpal  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            var transactionItem = (from s in db.tbl_PreHarmonization_ForOpal /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
                                   select new PreHarmonizationViewModel()
                                   {
                                       Amount = s.Amount,
                                       Vat = s.Vat,
                                       Fee = s.Fee,
                                       TransID = s.TransID,
                                       SrcAcctNo = s.SrcAcctNo,
                                       SrcInstCode = s.SrcInstCode,
                                       SrcInstBranchCode = s.SrcInstBranchCode,
                                       SrcInstType = s.SrcInstType,
                                       SrcInstUniqueID = s.SrcInstUniqueID,
                                       DestAcctNo = s.DestAcctNo,
                                       DestInstCode = s.DestInstCode,
                                       DestInstBranchCode = s.DestInstBranchCode,
                                       DestInstType = s.DestInstType,
                                       DestInstUniqueID = s.DestInstUniqueID,
                                       BankIncome = s.BankIncome,
                                       TransDate = s.TransDate,
                                       PsspParty = s.PsspParty,
                                       AccountType = s.AccountType,
                                       AccountClass = s.AccountClass,
                                       AccountDesignation = s.AccountDesignation,
                                       Currency = s.Currency,
                                       PaymentType = s.PaymentType,
                                       Channels = s.Channels,
                                       TransactionTypeCode = s.TransactionTypeCode,
                                       CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
                                       PepDesignatedAccount = s.PepDesignatedAccount,
                                       StampDutyExempt = s.StampDutyExempt,
                                       Inflow = s.Inflow,
                                       Emtl = s.Emtl,
                                       ReceiverLocation = s.ReceiverLocation,
                                       Status = s.Status,
                                       Id = s.Id,
                                   });

            return transactionItem;
        }



        public string SendForEbbsChecker(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_EbbsChecker where TransID in (select top 200000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                //string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
                //if (cnnString.ToLower().StartsWith("metadata="))
                //{
                //    System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                //    cnnString = efBuilder.ProviderConnectionString;
                //}
                //SqlConnection cnn = new SqlConnection(cnnString);
                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = cnn;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "sp_batchSending";
                //cnn.Open();
                //object o = cmd.ExecuteScalar();
                List<int> dist = db.tbl_PreHarmonization_EbbsChecker.Select(x => x.BatchCode).Distinct().ToList();
                //cnn.Close();

                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_EbbsChecker
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                        var audit = new TBL_AUDIT()
                        {
                            URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendBulkData",
                            UserName = objAudit.UserName,
                            DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = objAudit.UserId
                        };
                        db.Tbl_SendData.Add(data);
                        db.TBL_AUDIT.Add(audit);
                        db.SaveChanges();
                    }
                    message = "Sent Successfully";
                }
                
            }
            return message;
        }

        public string SendForEbbsMaker(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_EbbsMaker where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                List<int> dist = db.tbl_PreHarmonization_EbbsMaker.Select(x => x.BatchCode).Distinct().ToList();
                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_EbbsMaker
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                        var audit = new TBL_AUDIT()
                        {
                            URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendBulkData",
                            UserName = objAudit.UserName,
                            DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = objAudit.UserId
                        };
                        db.Tbl_SendData.Add(data);
                        db.TBL_AUDIT.Add(audit);
                        db.SaveChanges();
                    }
                    message = "Sent Successfully";
                }
            }
            return message;
        }

        public string SendForOpalChecker(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_OpalChecker where TransID in (select top 200000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                List<int> dist = db.tbl_PreHarmonization_OpalChecker.Select(x => x.BatchCode).Distinct().ToList();
                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_OpalChecker
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                        var audit = new TBL_AUDIT()
                        {
                            URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendBulkData",
                            UserName = objAudit.UserName,
                            DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = objAudit.UserId
                        };
                        db.Tbl_SendData.Add(data);
                        db.TBL_AUDIT.Add(audit);
                        db.SaveChanges();
                    }
                    message = "Sent Successfully";
                }
            }
            return message;
        }


        public string SendForOpalMaker(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_OpalMaker where TransID in (select top 200000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                List<int> dist = db.tbl_PreHarmonization_OpalMaker.Select(x => x.BatchCode).Distinct().ToList();
                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_OpalMaker
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                        var audit = new TBL_AUDIT()
                        {
                            URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendBulkData",
                            UserName = objAudit.UserName,
                            DETAIL = $"Transaction of transactionId '{item.Data}' has response of '{item.Description}'",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = objAudit.UserId
                        };
                        db.Tbl_SendData.Add(data);
                        db.TBL_AUDIT.Add(audit);
                        db.SaveChanges();
                    }
                    message = "Sent Successfully";
                }

            }
            return message;
        }

        public string SendForEbbs(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_ForEbbs where TransID in (select top 200000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                List<int> dist = db.tbl_PreHarmonization_ForEbbs.Select(x => x.BatchCode).Distinct().ToList();

                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_ForEbbs
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                            ReportingDate = db.tbl_PreHarmonization_ForEbbs.FirstOrDefault(t => t.TransID == item.Data).TransDate
                        };
                        db.Tbl_SendData.Add(data);
                        db.SaveChanges();
                    }
                    //StoreResponse(corpResponse);
                }
                //var audit = new TBL_AUDIT()
                //{
                //    URL = "https://localhost:44365/WholeTransaction/SendBulkForEbbsOpal",
                //    UserName = objAudit.UserName,
                //    DETAIL = $"Transactions sent to NIBBS by {objAudit.UserName}",
                //    IPADDRESS = SendEmail.GetLocalIpAddress(),
                //    DEVICENAME = SendEmail.GetDeviceName(),
                //    OSNAME = SendEmail.FriendlyName(),
                //    DATETIME = DateTime.UtcNow,
                //    UserId = objAudit.UserId
                //};
                //db.TBL_AUDIT.Add(audit);
                //db.SaveChanges();
                message = "Sent Successfully";
            }
            return message;
        }
        public string SendForOpal(TBL_AUDIT objAudit)
        {
            string message = String.Empty;
            db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization_ForOpal where TransID in (select top 200000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            Token token = new Token();
            SendBulkResult dataResp = new SendBulkResult();
            NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
            Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
            string grant_type = "client_credentials";
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
                List<int> dist = db.tbl_PreHarmonization_ForOpal.Select(x => x.BatchCode).Distinct().ToList();

                foreach (int i in dist)
                {
                    TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_ForOpal
                                                     where s.BatchCode == i && s.Status == "Pending"
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
                            ReportingDate = db.tbl_PreHarmonization_ForOpal.FirstOrDefault(t => t.TransID == item.Data).TransDate
                        };
                        db.Tbl_SendData.Add(data);
                        db.SaveChanges();
                    }
                    //StoreResponse(corpResponse);
                }
                //var audit = new TBL_AUDIT()
                //{
                //    URL = "https://localhost:44365/WholeTransaction/SendBulkForEbbsOpal",
                //    UserName = objAudit.UserName,
                //    DETAIL = $"Transactions sent to NIBBS by {objAudit.UserName}",
                //    IPADDRESS = SendEmail.GetLocalIpAddress(),
                //    DEVICENAME = SendEmail.GetDeviceName(),
                //    OSNAME = SendEmail.FriendlyName(),
                //    DATETIME = DateTime.UtcNow,
                //    UserId = objAudit.UserId
                //};
                //db.TBL_AUDIT.Add(audit);
                //db.SaveChanges();
                message = "Sent Successfully";
            }
            return message;
        }
        public string SendSingleEbbsChecker(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_EbbsChecker.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_EbbsChecker objToSend = db.tbl_PreHarmonization_EbbsChecker.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                    string grant_type = "client_credentials";
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
                            URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendSingle",
                            UserName = objAudit.UserName,
                            DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                            IPADDRESS = SendEmail.GetLocalIpAddress(),
                            DEVICENAME = SendEmail.GetDeviceName(),
                            OSNAME = SendEmail.FriendlyName(),
                            DATETIME = DateTime.UtcNow,
                            UserId = objAudit.UserId
                        };
                    message = postResp.Description;
                        db.TBL_AUDIT.Add(audit);
                        db.Tbl_SendData.Add(data);
                        db.SaveChanges();
                    }
                    //return Json(postResp.Description, JsonRequestBehavior.AllowGet);
                }
            return message;
        }
        public string SendSingleEbbsMaker(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_EbbsMaker.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_EbbsMaker objToSend = db.tbl_PreHarmonization_EbbsMaker.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string grant_type = "client_credentials";
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
                        URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendSingle",
                        UserName = objAudit.UserName,
                        DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = objAudit.UserId
                    };
                    message = postResp.Description;
                    db.TBL_AUDIT.Add(audit);
                    db.Tbl_SendData.Add(data);
                    db.SaveChanges();
                }
            }
            return message;
        }
        public string SendSingleOpalChecker(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_OpalChecker.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_OpalChecker objToSend = db.tbl_PreHarmonization_OpalChecker.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string grant_type = "client_credentials";
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
                        URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendSingle",
                        UserName = objAudit.UserName,
                        DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = objAudit.UserId
                    };
                    message = postResp.Description;
                    db.TBL_AUDIT.Add(audit);
                    db.Tbl_SendData.Add(data);
                    db.SaveChanges();
                }
            }
            return message;
        }
        public string SendSingleOpalMaker(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_OpalMaker.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_OpalMaker objToSend = db.tbl_PreHarmonization_OpalMaker.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string grant_type = "client_credentials";
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
                        URL = "https://localhost:44365/" + "WholeTransaction/GeneralSendSingle",
                        UserName = objAudit.UserName,
                        DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = objAudit.UserId
                    };
                    message = postResp.Description;
                    db.TBL_AUDIT.Add(audit);
                    db.Tbl_SendData.Add(data);
                    db.SaveChanges();
                }
            }
            return message;
        }

        public string SendSingleForEbbs(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_ForEbbs.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_ForEbbs objToSend = db.tbl_PreHarmonization_ForEbbs.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string grant_type = "client_credentials";
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
                        ReportingDate = db.tbl_PreHarmonization_ForEbbs.FirstOrDefault(t => t.TransID == postResp.Data).TransDate
                    };
                    if (postResp.Status == "00")
                    {
                        objToSend.Status = "Sent";
                    }
                    else
                    {
                        objToSend.Status = "Pending";
                    }
                    //var audit = new TBL_AUDIT()
                    //{
                    //    URL = "https://localhost:44365/" + "WholeTransaction/SendSingleForEbbsOpal",
                    //    UserName = objAudit.UserName,
                    //    DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                    //    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    //    DEVICENAME = SendEmail.GetDeviceName(),
                    //    OSNAME = SendEmail.FriendlyName(),
                    //    DATETIME = DateTime.UtcNow,
                    //    UserId = objAudit.UserId
                    //};

                    //db.TBL_AUDIT.Add(audit);
                    db.Tbl_SendData.Add(data);
                    db.SaveChanges();
                    message = postResp.Description;
                }
            }
            return message;
        }
        public string SendSingleForOpal(IndividualTransaction Id, TBL_AUDIT objAudit)
        {
            NIBBSPortalResponse postResp = new NIBBSPortalResponse();
            Token token = new Token();
            SendSingleResult dataResp = new SendSingleResult();
            string message = String.Empty;
            string transId = db.tbl_PreHarmonization_ForOpal.FirstOrDefault(x => x.Id == Id.Id).TransID;
            tbl_PreHarmonization_ForOpal objToSend = db.tbl_PreHarmonization_ForOpal.FirstOrDefault(x => x.TransID == transId);
            bool dataSent = db.Tbl_SendData.Any(x => x.Message.Equals("SUCCESSFUL", StringComparison.OrdinalIgnoreCase) && x.Data.Equals(objToSend.TransID));
            if (dataSent)
            {
                message = "There's already an existing record with transactionId " + transId + " at NIBSS endpoint";
                return message;
            }
            else
            {
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
                string grant_type = "client_credentials";
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
                        ReportingDate = db.tbl_PreHarmonization_ForOpal.FirstOrDefault(t => t.TransID == postResp.Data).TransDate
                    };
                    if (postResp.Status == "00")
                    {
                        objToSend.Status = "Sent";
                    }
                    else
                    {
                        objToSend.Status = "Pending";
                    }
                    //var audit = new TBL_AUDIT()
                    //{
                    //    URL = "https://localhost:44365/" + "WholeTransaction/SendSingleForEbbsOpal",
                    //    UserName = objAudit.UserName,
                    //    DETAIL = $"Transaction of transactionId '{postResp.Data}' has response of {postResp.Description} sent to NIBSS",
                    //    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    //    DEVICENAME = SendEmail.GetDeviceName(),
                    //    OSNAME = SendEmail.FriendlyName(),
                    //    DATETIME = DateTime.UtcNow,
                    //    UserId = objAudit.UserId
                    //};
                    message = postResp.Description;
                    //db.TBL_AUDIT.Add(audit);
                    db.Tbl_SendData.Add(data);
                    db.SaveChanges();
                }
            }
            return message;
        }

        public void StoreResponse(string corpResponse)
        {
            //SendDataViewModel[] data = (from s in entities.Tbl_SendData
            //                            select new SendDataViewModel
            //                            {
            //                                Status = s.Status,
            //                                Message = s.Message,
            //                                Description = s.Description,
            //                                Data = s.Data
            //                            }).ToArray();
            //string stringDatab = JsonConvert.SerializeObject(data);
            string myPath = @"C:\IDH\NIBBSResponse\";
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string fileExtension = ".json";
            string fileName = "nibbsResponseFile_";
            string filepath = String.Concat(myPath, fileName, Todaysdate, fileExtension);
            bool pathExist = System.IO.File.Exists(filepath);
            if (!pathExist)
            {
                System.IO.File.Create(filepath).Close();
            }
            System.IO.File.AppendAllText(filepath, corpResponse);
        }

    }
}