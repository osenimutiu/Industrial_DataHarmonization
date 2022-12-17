using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class EXController : Controller
    {
        private SCBDBEntities objDbEntities;
        // private  readonly IExceptionManager _exceptionManager;

        public EXController()
        {
            objDbEntities = new SCBDBEntities();

        }
        // GET: EX
        public ActionResult Index()
        {
            tbl_ExViewModel objectRoomViewModel = new tbl_ExViewModel();
            objectRoomViewModel.ListOfAccountClass = (from obj in objDbEntities.tbl_AccountClass
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.AccountClassTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfAccountType = (from obj in objDbEntities.tbl_AccountType
                                                     select new SelectListItem()
                                                     {
                                                         Text = obj.AccountTypes,
                                                         Value = obj.Id.ToString()


                                                     }).ToList();
            objectRoomViewModel.ListOfAccountDesignation = (from obj in objDbEntities.tbl_AccountDesignation
                                                            select new SelectListItem()
                                                            {
                                                                Text = obj.AccountDesignationTypes,
                                                                Value = obj.Id.ToString()


                                                            }).ToList();
            objectRoomViewModel.ListOfSrctInstType = (from obj in objDbEntities.tbl_SrcInsType
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.SrcInstTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfDestInstType = (from obj in objDbEntities.tbl_DestInstType
                                                      select new SelectListItem()
                                                      {
                                                          Text = obj.DestInstTypes,
                                                          Value = obj.Id.ToString()


                                                      }).ToList();
            objectRoomViewModel.ListOfPaymentType = (from obj in objDbEntities.tbl_PaymentTypes
                                                     select new SelectListItem()
                                                     {
                                                         Text = obj.PaymentTypes,
                                                         Value = obj.Id.ToString()


                                                     }).ToList();
            objectRoomViewModel.ListOfCurrency = (from obj in objDbEntities.tbl_Currency
                                                  select new SelectListItem()
                                                  {
                                                      Text = obj.CurrencyTypes,
                                                      Value = obj.Id.ToString()


                                                  }).ToList();
            objectRoomViewModel.ListOfChannels = (from obj in objDbEntities.tbl_Channel
                                                  select new SelectListItem()
                                                  {
                                                      Text = obj.ChannelTypes,
                                                      Value = obj.Id.ToString()


                                                  }).ToList();
            return View(objectRoomViewModel);
        }

        [HttpPost]
        public ActionResult Index(tbl_ExViewModel objRoomViewModel)
        {
            string message = string.Empty;
            if (objRoomViewModel.Id != 0)
            {
                tbl_Exceptionb objRoom = objDbEntities.tbl_Exceptionb.Single(model => model.Id == objRoomViewModel.Id);
                objRoom.Amount = objRoomViewModel.Amount;
                objRoom.Vat = objRoomViewModel.Vat;
                objRoom.Fee = objRoomViewModel.Fee;
                objRoom.TransID = objRoomViewModel.TransID;
                objRoom.SrcAcctNo = objRoomViewModel.SrcAcctNo;
                objRoom.SrcInstCode = objRoomViewModel.SrcInstCode;
                objRoom.SrcInstBranchCode = objRoomViewModel.SrcInstBranchCode;
                objRoom.SrcInstType = objRoomViewModel.SrcInstType;
                objRoom.SrcInstUniqueID = objRoomViewModel.SrcInstUniqueID;
                objRoom.DestAcctNo = objRoomViewModel.DestAcctNo;
                objRoom.DestInstCode = objRoomViewModel.DestInstCode;
                objRoom.DestInstBranchCode = objRoomViewModel.DestInstBranchCode;
                objRoom.DestInstType = objRoomViewModel.DestInstType;
                objRoom.DestInstUniqueID = objRoomViewModel.DestInstUniqueID;
                objRoom.PaymentType = objRoomViewModel.PaymentType;
                objRoom.BankIncome = objRoomViewModel.BankIncome;
                objRoom.TransDate = objRoomViewModel.TransDate;
                objRoom.PsspParty = objRoomViewModel.PsspParty;
                objRoom.AccountType = objRoomViewModel.AccountType;
                objRoom.AccountClass = objRoomViewModel.AccountClass;
                objRoom.AccountDesignation = objRoomViewModel.AccountDesignation;
                objRoom.Currency = objRoomViewModel.Currency;
                objRoom.Channels = objRoomViewModel.Channels;
                objRoom.TransactionTypeCode = objRoomViewModel.TransactionTypeCode;
                objRoom.PepDesignatedAccount = objRoomViewModel.PepDesignatedAccount;
                objRoom.CypherSecurityLevyExempt = objRoomViewModel.CypherSecurityLevyExempt;
                objRoom.StampDutyExempt = objRoomViewModel.StampDutyExempt;
                objRoom.ExceptionType = objRoomViewModel.ExceptionType;
                objRoom.AdditionalField = objRoomViewModel.AdditionalField;
                objRoom.Inflow = objRoomViewModel.Inflow;
                message = "Updated";
            }
            objDbEntities.SaveChanges();
            return Json(new { message = "Exception Succefully " + message, success = true }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAllExceptions()
        {
            IEnumerable<tbl_ExDetailsViewModel> listOfRoomDetailsViewModels =
                (
                from objRoom in objDbEntities.tbl_Exceptionb
                select new tbl_ExDetailsViewModel()
                {
                    Amount = objRoom.Amount,
                    Vat = objRoom.Vat,
                    Fee = objRoom.Fee,
                    TransID = objRoom.TransID,
                    SrcAcctNo = objRoom.SrcAcctNo,
                    SrcInstCode = objRoom.SrcInstCode,
                    SrcInstBranchCode = objRoom.SrcInstBranchCode,
                    SrcInstTypes = objRoom.SrcInstType.ToString(),
                    SrcInstUniqueID = objRoom.SrcInstUniqueID,
                    DestAcctNo = objRoom.DestAcctNo,
                    DestInstCode = objRoom.DestInstCode,
                    DestInstBranchCode = objRoom.DestInstBranchCode,
                    DestInstTypes = objRoom.DestInstType.ToString(),
                    DestInstUniqueID = objRoom.DestInstUniqueID,
                    PaymentTypes = objRoom.PaymentType.ToString(),
                    BankIncome = objRoom.BankIncome,
                    TransDate = objRoom.TransDate,
                    PsspParty = objRoom.PsspParty,
                    AccountTypes = objRoom.AccountType.ToString(),
                    AccountClassTypes = objRoom.AccountClass.ToString(),
                    AccountDesignationTypes = objRoom.AccountDesignation.ToString(),
                    CurrencyTypes = objRoom.Currency.ToString(),
                    ChannelTypes = objRoom.Channels.ToString(),
                    TransactionTypeCode = objRoom.TransactionTypeCode,
                    PepDesignatedAccount = objRoom.PepDesignatedAccount,
                    CypherSecurityLevyExempt = objRoom.CypherSecurityLevyExempt,
                    StampDutyExempt = objRoom.StampDutyExempt,
                    ExceptionType = objRoom.ExceptionType,
                    AdditionalField = objRoom.AdditionalField,
                    Inflow = objRoom.Inflow,
                    Id = objRoom.Id

                }).ToList();
            return PartialView("_ExDetailsPartial", listOfRoomDetailsViewModels);
        }

        [HttpGet]
        public JsonResult EditExceptionDetails(int roomId)
        {
            var result = objDbEntities.tbl_Exceptionb.Single(model => model.Id == roomId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteExceptionDetails(int roomId, tbl_Exceptionb exceptionb)
        {
            tbl_Exceptionb objRoom = objDbEntities.tbl_Exceptionb.Single(model => model.Id == roomId);

            objDbEntities.SaveChanges();
            return Json(new { mssage = "Exception Successfully Deleted.", success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckException()
        {

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
            cmd.CommandText = "sp_exception";
            cnn.Open();
            object o = cmd.ExecuteScalar();
            ViewBag.Message = "Exception Verified Successfully";
            cnn.Close();
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} checked exception",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult FixException()
        {
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
            cmd.CommandText = "sp_fixexception";
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            ViewBag.Message = "Exception Fixed Successfully";
            cnn.Close();
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} has fixed exception successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[31] {
                                                     new DataColumn("ExceptionType"),
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
                                                     new DataColumn("TenantId"),
                                                    new DataColumn("AdditionalField"),
                                                     new DataColumn("Inflow")});

            var objException = from e in objDbEntities.tbl_Exceptionb select e;

            foreach (var item in objException)
            {
                dt.Rows.Add
                    (
                    item.ExceptionType,
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
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"User {User.Identity.Name} exports exception successfully",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                    objDbEntities.SaveChanges();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exception.xlsx");
                }
            }
        }

       


    }
}