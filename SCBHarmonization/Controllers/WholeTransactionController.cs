using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using OfficeOpenXml;
using PagedList;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.NibssModels;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class WholeTransactionController : Controller
    {
        private SCBDBEntities db = new SCBDBEntities();
        string baseAddress = "https://api.nibss-plc.com.ng";
        RoleBasedTransaction roleTransaction = new RoleBasedTransaction();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }
        //public WholeTransactionController()
        //{
        //    db = new SCBDBEntities();
        //}
        // GET: WholeTransaction
        //public ActionResult GetTransaction()
        //{
        //    db.Database.ExecuteSqlCommand("update tbl_PreHarmonization set Status = 'Sent' where TransID in (select top (15000) data from tbl_SendData where Status = '00' order by id desc)");

        //    var transactionItem = (from s in db.tbl_PreHarmonization /*join t in db.Tbl_SendData on s.TransID equals t.Data*/
        //                           select new PreHarmonizationViewModel()
        //    {
        //        Amount = s.Amount,
        //        Vat = s.Vat,
        //        Fee = s.Fee,
        //        TransID = s.TransID,
        //        SrcAcctNo = s.SrcAcctNo,
        //        SrcInstCode = s.SrcInstCode,
        //        SrcInstBranchCode = s.SrcInstBranchCode,
        //        SrcInstType = s.SrcInstType,
        //        SrcInstUniqueID = s.SrcInstUniqueID,
        //        DestAcctNo = s.DestAcctNo,
        //        DestInstCode = s.DestInstCode,
        //        DestInstBranchCode = s.DestInstBranchCode,
        //        DestInstType = s.DestInstType,
        //        DestInstUniqueID = s.DestInstUniqueID,
        //        BankIncome = s.BankIncome,
        //        TransDate = s.TransDate,
        //        PsspParty = s.PsspParty,
        //        AccountType = s.AccountType,
        //        AccountClass = s.AccountClass,
        //        AccountDesignation = s.AccountDesignation,
        //        Currency = s.Currency,
        //        PaymentType = s.PaymentType,
        //        Channels = s.Channels,
        //        TransactionTypeCode = s.TransactionTypeCode,
        //        CypherSecurityLevyExempt = s.CypherSecurityLevyExempt,
        //        PepDesignatedAccount = s.PepDesignatedAccount,
        //        StampDutyExempt = s.StampDutyExempt,
        //        Inflow = s.Inflow,
        //        Emtl = s.Emtl,
        //        ReceiverLocation = s.ReceiverLocation,
        //        Status = s.Status,
        //        Id = s.Id,
        //    }).Take(50).ToArray();
        //    return View(transactionItem);
        //}

        public async Task<ActionResult> GetTransaction(string sortOrder, string currentFilter, string search, int? page)
        {
            //List<PreHarmonizationViewModel> result = new List<PreHarmonizationViewModel>();
            //db.Database.ExecuteSqlCommand("update tbl_PreHarmonization  set status = 'Sent' where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            IQueryable<PreHarmonizationViewModel> transactionItem = null;


            var UserID = User.Identity.GetUserId();
            var role = await UserManager.GetRolesAsync(UserID);
            string singleRole = role[0];
            if(singleRole.ToLower().StartsWith("ebbs"))
            {
                ViewBag.RecordRelated = "eBBS related records";
            }
            else if(singleRole.ToLower().StartsWith("opal"))
            {
                ViewBag.RecordRelated = "Opal related records";
            }
            //var role =  UserManager.GetRoles(UserID);
            //string[] sffg = Roles.GetUsersInRole("System Admin");
            //if (role.Contains("eBBS-Checker"))
            //{
            //     transactionItem = roleTransaction.GetEbbsCheckerTransaction();
            //}
            //else if (role.Contains("eBBS-Maker"))
            //{
            //     transactionItem = roleTransaction.GetEbbsMakerTransaction();
            //}
            //else if (role.Contains("Opal-Checker"))
            //{
            //     transactionItem = roleTransaction.GetOpalCheckerTransaction();
            //}
            //if (role.Contains("Opal-Maker"))
            //{
            //     transactionItem = roleTransaction.GetOpalMakerTransaction();
            //}

            switch (singleRole)
            {
                case "eBBS-Checker":
                    transactionItem = roleTransaction.GetEbbsTransaction();
                    break;
                case "eBBS-Maker":
                    transactionItem = roleTransaction.GetEbbsTransaction();
                    break;
                case "Opal-Checker":
                    transactionItem = roleTransaction.GetOpalTransaction();
                    break;
                default: 
                    transactionItem = roleTransaction.GetOpalTransaction();
                    break;
            }
            try
            {
                if (search == null)
                {
                    page = 1;
                    search = currentFilter;

                    ViewBag.TotalCount = transactionItem.Count();
                    transactionItem = transactionItem.Take(1000);
                    ViewBag.RecordCount = transactionItem.Count();
                    //return View(transactionItem);
                    //result = transactionItem;
                }
                else
                {
                    //return View(db.StudentInfoes.Where(t => t.Name.Contains(search)).ToList());
                    page = 1;
                    ViewBag.TotalCount = transactionItem.Count();
                    transactionItem = transactionItem.Where(t => t.TransID.Contains(search));
                    ViewBag.RecordCount = transactionItem.Count();
                    //result = transactionItemb;
                    //return View(transactionItem);
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        transactionItem = transactionItem.OrderByDescending(s => s.Id);
                        break;
                    case "Date":
                        transactionItem = transactionItem.OrderBy(s => s.Id);
                        break;
                    case "date_desc":
                        transactionItem = transactionItem.OrderByDescending(s => s.Id);
                        break;
                    default:  // Name ascending 
                        transactionItem = transactionItem.OrderBy(s => s.Id);
                        break;
                }
            }
            catch (Exception)
            {

            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(transactionItem.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<JsonResult> GeneralSendBulkData(CheckExisting transId)
        {
            var UserID = User.Identity.GetUserId();
            var role = await UserManager.GetRolesAsync(UserID);
            string singleRole = role[0];
            string message = String.Empty;
            TBL_AUDIT audit = new TBL_AUDIT()
            {
                UserName = User.Identity.Name,
                UserId = User.Identity.GetUserId()
            };
            try
            {
                switch (singleRole)
                {
                    case "eBBS-Checker":
                        message = roleTransaction.SendForEbbsChecker(audit);
                        break;
                    case "eBBS-Maker":
                        message = roleTransaction.SendForEbbsMaker(audit);
                        break;
                    case "Opal-Checker":
                        message = roleTransaction.SendForOpalChecker(audit);
                        break;
                    default:  // Name ascending 
                        message = roleTransaction.SendForOpalMaker(audit);
                        break;
                }
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "WholeTransaction",
                    ActionName = "GeneralSendBulkData",
                    InnerException = ex.InnerException.InnerException.Message,
                    LogTime = DateTime.Now
                };
                db.ExceptionLoggers.Add(logger);
                db.SaveChanges();
                message = ex.InnerException.InnerException.Message;
                //return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SendBulkForEbbsOpal(CheckExisting transId)
        {
            var UserID = User.Identity.GetUserId();
            var role = await UserManager.GetRolesAsync(UserID);
            string singleRole = role[0];
            string message = String.Empty;
            TBL_AUDIT audit = new TBL_AUDIT()
            {
                UserName = User.Identity.Name,
                UserId = User.Identity.GetUserId()
            };
            try
            {
                switch (singleRole)
                {
                    case "eBBS-Checker":
                        message = roleTransaction.SendForEbbs(audit);
                        break;
                    case "eBBS-Maker":
                        message = roleTransaction.SendForEbbs(audit);
                        break;
                    case "Opal-Checker":
                        message = roleTransaction.SendForOpal(audit);
                        break;
                    default:  // Name ascending 
                        message = roleTransaction.SendForOpal(audit);
                        break;
                }
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "WholeTransaction",
                    ActionName = "SendBulkDataForEbbs",
                    InnerException = ex.InnerException.InnerException.Message,
                    LogTime = DateTime.Now
                };
                db.ExceptionLoggers.Add(logger);
                db.SaveChanges();
                message = ex.InnerException.InnerException.Message;
                //return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SendBulkData(CheckExisting transId)
        {
            string message = String.Empty;
            try
            {
                db.Database.ExecuteSqlCommand("delete from tbl_PreHarmonization where TransID in (select top 600000 Data from Tbl_SendData where status = '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)");
                Token token = new Token();
                SendBulkResult dataResp = new SendBulkResult();
                NIBBSPortalResponse[] postResp = new NIBBSPortalResponse[1000000];
                Tbl_Credential cred = db.Tbl_Credential.OrderByDescending(x => x.Id).FirstOrDefault();
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
                    List<int> dist = db.tbl_PreHarmonization_Batch.Select(x => x.BatchCode).Distinct().ToList();
                    cnn.Close();

                    foreach (int i in dist)
                    {
                        TransactionRequest[] dataList = (from s in db.tbl_PreHarmonization_Batch
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
                        //Json Storage Path

                        //string path = @"C:\NIBBResponseJsonFile";
                        //bool pathExist = System.IO.Directory.Exists(path);
                        //if (!pathExist)

                        //{
                        //    System.IO.Directory.CreateDirectory(path);
                        //}
                        //System.IO.File.AppendAllText(path, corpResponse);

                        //Json Storage Path End
                        foreach (var item in postResp)
                        {
                            string TransId = db.tbl_PreHarmonization.FirstOrDefault(x => x.TransID == item.Data).TransDate;
                            var data = new Tbl_SendData
                            {
                                Status = item.Status,
                                Message = item.Message,
                                Description = item.Description,
                                Data = item.Data,
                                SentDate = TransId
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
                            //db.TBL_AUDIT.Add(audit);
                            db.Tbl_SendData.Add(data);
                            db.SaveChanges();
                        }
                    }

                    message = "Sent Successfully";
                    ViewBag.Message = message;
                }
            }
            catch (Exception ex)
            {

                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "WholeTransaction",
                    ActionName = "SendBulkData",
                    InnerException = ex.InnerException.InnerException.Message,
                    LogTime = DateTime.Now
                };
                db.ExceptionLoggers.Add(logger);
                db.SaveChanges();
                return Json(ex.InnerException.InnerException.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GeneralSendSingle(IndividualTransaction Id)
        {
            var UserID = User.Identity.GetUserId();
            var role = await UserManager.GetRolesAsync(UserID);
            string singleRole = role[0];
            string message = String.Empty;
            TBL_AUDIT audit = new TBL_AUDIT()
            {
                UserName = User.Identity.Name,
                UserId = User.Identity.GetUserId()
            };
            try
            {
                switch (singleRole)
                {
                    case "eBBS-Checker":
                        message = roleTransaction.SendSingleEbbsChecker(Id, audit);
                        break;
                    case "eBBS-Maker":
                        message = roleTransaction.SendSingleEbbsMaker(Id, audit);
                        break;
                    case "Opal-Checker":
                        message = roleTransaction.SendSingleOpalChecker(Id,audit);
                        break;
                    default:  // Name ascending 
                        message = roleTransaction.SendSingleOpalMaker(Id,audit);
                        break;
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
                message = ex.Message;
                db.ExceptionLoggers.Add(logger);
                db.SaveChanges();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SendSingleForEbbsOpal(IndividualTransaction Id)
        {
            var UserID = User.Identity.GetUserId();
            var role = await UserManager.GetRolesAsync(UserID);
            string singleRole = role[0];
            string message = String.Empty;
            TBL_AUDIT audit = new TBL_AUDIT()
            {
                UserName = User.Identity.Name,
                UserId = User.Identity.GetUserId()
            };
            try
            {
                switch (singleRole)
                {
                    case "eBBS-Checker":
                        message = roleTransaction.SendSingleForEbbs(Id, audit);
                        break;
                    case "eBBS-Maker":
                        message = roleTransaction.SendSingleForEbbs(Id, audit);
                        break;
                    case "Opal-Checker":
                        message = roleTransaction.SendSingleForOpal(Id, audit);
                        break;
                    default:  // Name ascending 
                        message = roleTransaction.SendSingleForOpal(Id, audit);
                        break;
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
                message = ex.Message;
                db.ExceptionLoggers.Add(logger);
                db.SaveChanges();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[29] {
                                                     new DataColumn("Amount"),
                                                     new DataColumn("Vat"),
                                                     new DataColumn("Fee"),
                                                     new DataColumn("TransID"),
                                                     new DataColumn("SrcAcctNo"),
                                                     new DataColumn("SrcInstCode"),
                                                     new DataColumn("SrcInstBranchCode"),
                                                     new DataColumn("SrcInstType"),
                                                     new DataColumn("SrcInstUniqueID"),
                                                     new DataColumn("DestAcctNo"),
                                                     new DataColumn("DestInstCode"),
                                                     new DataColumn("DestInstBranchCode"),
                                                     new DataColumn("DestInstType"),
                                                     new DataColumn("DestInstUniqueID"),
                                                     new DataColumn("PaymentType"),
                                                     new DataColumn("BankIncome"),
                                                     new DataColumn("TransDate"),
                                                     new DataColumn("PsspParty"),
                                                     new DataColumn("AccountType"),
                                                     new DataColumn("AccountClass"),
                                                     new DataColumn("AccountDesignation"),
                                                     new DataColumn("Currency"),
                                                     new DataColumn("Channels"),
                                                     new DataColumn("TransactionTypeCode"),
                                                     new DataColumn("PepDesignatedAccount"),
                                                     new DataColumn("CypherSecurityLevyExempt"),
                                                     new DataColumn("StampDutyExempt"),
                                                    new DataColumn("AdditionalField"),
                                                     new DataColumn("Inflow")});

            var objException = from e in db.tbl_PreHarmonization select e;

            foreach (var item in objException)
            {
                dt.Rows.Add
                    (
                    item.Amount,
                    item.Vat,
                    item.Fee,
                    item.TransID,
                    item.SrcAcctNo,
                    item.SrcInstCode,
                    item.SrcInstBranchCode,
                    item.SrcInstType,
                    item.SrcInstUniqueID,
                    item.DestAcctNo,
                    item.DestInstCode,
                    item.DestInstBranchCode,
                    item.DestInstType,
                    item.DestInstUniqueID,
                    item.PaymentType,
                    item.BankIncome,
                    item.TransDate,
                    item.PsspParty,
                    item.AccountType,
                    item.AccountClass,
                    item.AccountDesignation,
                    item.Currency,
                    item.Channels,
                    item.TransactionTypeCode,
                    item.PepDesignatedAccount,
                    item.CypherSecurityLevyExempt,
                    item.StampDutyExempt,
                    item.AdditionalField,
                    item.Inflow
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LoadedData.xlsx");
                }
            }
        }

        public void ExportListUsingEPPlus()
        {
            List<tbl_PreHarmonization> data = new List<tbl_PreHarmonization>();
            using (var context = new SCBDBEntities())
            {
                data = context.tbl_PreHarmonization.ToList();
            }

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(data, true);
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as Students.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=LoadedData.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }


        [HttpGet]
        public JsonResult GetLastTransDate()
        {
            var i = 1;
            var dataList = db.SaveLastLoadedDatas.OrderByDescending(s => s.Id).Take(1).ToList();
            var data = dataList.Select(x => new
            {
                Id = x.Id,
                TransDate = x.TransactionDate
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DownLoadResponse(CheckExisting transId)
        {
            string message = String.Empty;
            string nibbsResponseFile = @"C:\IDH\NIBBSResponse";
            //Tbl_SendData[] postResp = new Tbl_SendData[700000];
            try
            {
                using (SCBDBEntities db = new SCBDBEntities())
                {
                    var directory = new DirectoryInfo(nibbsResponseFile);
                    var myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                    StreamReader streamReader = new StreamReader(myFile.FullName);
                    string data = streamReader.ReadToEnd();
                    string replaceSuareBra = data.Replace(']', ',').Replace('[', ' ');
                    string wantedChar = "[" + replaceSuareBra + "]";
                    //List<Tbl_SendData> products = JsonConvert.DeserializeObject<List<Tbl_SendData>>(wantedChar);
                    Tbl_SendData[] postResp = JsonConvert.DeserializeObject<Tbl_SendData[]>(wantedChar);

                    //products.ForEach(p =>
                    //{
                    //    Tbl_SendData jsonData = new Tbl_SendData()
                    //    {
                    //        Status = p.Status,
                    //        Message = p.Message,
                    //        Description = p.Description,
                    //        Data = p.Data,
                    //        SentDate = DateTime.Now.ToString(),
                    //    };
                    //    db.Tbl_SendData.Add(jsonData);
                    //    db.SaveChanges();
                    //});
                    foreach (Tbl_SendData p in postResp)
                    {
                        Tbl_SendData jsonData = new Tbl_SendData()
                        {
                            Status = p.Status,
                            Message = p.Message,
                            Description = p.Description,
                            Data = p.Data,
                            SentDate = DateTime.Now.ToString(),
                        };
                        db.Tbl_SendData.Add(jsonData);
                        db.SaveChanges();
                    }
                    message = "Response uploaded Successfully!";
                }
            }
            catch (Exception ex)
            {

                message = ex.Message;
            }
            
            return Json(message,JsonRequestBehavior.AllowGet);
        }
   
    }
}