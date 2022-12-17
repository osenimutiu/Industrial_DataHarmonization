using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using SCBHarmonization.Models;
using ClosedXML.Excel;
using Rotativa;
using SCBHarmonization.Helper;
using Microsoft.AspNet.Identity;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    public class ThirdImportController : Controller
    {
        SCBDBEntities entities = new SCBDBEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        OleDbConnection Econ;
        // GET: ThirdImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string name)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/excelfolder/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
            InsertExceldata(filepath, filename);
            ViewBag.Message = string.Format("Saved Successfully {0}", name, DateTime.Now.ToString());
            return View();
        }

         private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        } 

        private ActionResult InsertExceldata(string fileepath, string filename)
        {
            entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_RawHarmonizationDump] ");
            entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_FirstHarmonizationDump] ");
            //entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_PreHarmonization] ");
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Sheet1$");
            //string query = string.Format("Select * from [{0}]", "Book1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
             DataTable dt = ds.Tables[0];
            //DataTable dt = new DataTable();
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "tbl_RawHarmonizationDump";
            objbulk.ColumnMappings.Add("Amount", "Amount");
            objbulk.ColumnMappings.Add("Vat", "Vat");
            objbulk.ColumnMappings.Add("Fee", "Fee");
            objbulk.ColumnMappings.Add("TransID", "TransID");
            objbulk.ColumnMappings.Add("SrcAcctNo", "SrcAcctNo");
            objbulk.ColumnMappings.Add("SrcInstCode", "SrcInstCode");
            objbulk.ColumnMappings.Add("SrcInstBranchCode", "SrcInstBranchCode");
            objbulk.ColumnMappings.Add("SrcInstType", "SrcInstType");
            objbulk.ColumnMappings.Add("SrcInstUniqueID", "SrcInstUniqueID");
            objbulk.ColumnMappings.Add("DestAccNo", "DestAcctNo");
            objbulk.ColumnMappings.Add("DestInstCode", "DestInstCode");
            objbulk.ColumnMappings.Add("DestInstBranchCode", "DestInstBranchCode");
            objbulk.ColumnMappings.Add("DestInstType", "DestInstType");
            objbulk.ColumnMappings.Add("DestInstUniqueID", "DestInstUniqueID");
            objbulk.ColumnMappings.Add("PaymentType", "PaymentType");
            objbulk.ColumnMappings.Add("BankIncome", "BankIncome");
            objbulk.ColumnMappings.Add("TransDate", "TransDate");
            objbulk.ColumnMappings.Add("PsspParty", "PsspParty");
            objbulk.ColumnMappings.Add("AccountType", "AccountType");
            objbulk.ColumnMappings.Add("AccountClass", "AccountClass");
            objbulk.ColumnMappings.Add("AccountDesignation", "AccountDesignation");
            objbulk.ColumnMappings.Add("Currency", "Currency");
            objbulk.ColumnMappings.Add("Channels", "Channels");
            objbulk.ColumnMappings.Add("TransactionTypeCode", "TransactionTypeCode");
            objbulk.ColumnMappings.Add("PepDesignatedAccount", "PepDesignatedAccount");
            objbulk.ColumnMappings.Add("CyberSecurityLevyExempt", "CypherSecurityLevyExempt");
            objbulk.ColumnMappings.Add("StampDutyExempt", "StampDutyExempt");
            //objbulk.ColumnMappings.Add("TenantId", "TenantId");
            objbulk.ColumnMappings.Add("AdditionalField", "AdditionalField");
            objbulk.ColumnMappings.Add("Inflow", "Inflow");
            objbulk.ColumnMappings.Add("Emtl", "Emtl");
            objbulk.ColumnMappings.Add("ReceiverLocation", "ReceiverLocation");
            SqlCommand cmdd = new SqlCommand();
            cmdd.Connection = con;
            cmdd.CommandType = System.Data.CommandType.StoredProcedure;
            cmdd.CommandText = "RawExcel_DataTransfer";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Excel_DataTransfer";
            con.Open();
            objbulk.WriteToServer(dt);
            object o = cmdd.ExecuteScalar();
            object r = cmd.ExecuteScalar();
            SaveTransaction();
            con.Close();
            return RedirectToAction("Index");

        }


        public ActionResult RunPackage()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString;

            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Run_execute_ssis_package";
                //cmd.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "output_execution_id",
                //    Value = ""//ValueVariable or varchar;
                //});
                //add any parameters the stored procedure might require
                cnn.Open();
                object o = cmd.ExecuteScalar();
                cnn.Close();
            }
            ViewBag.Message = "Data Loading Successfully";
            return View();

        }


        [HttpPost]
        public JsonResult ExecutePackage(PackageEntity att)
        {
            bool result = false;
            try
            {
                ExecutePackage package = new ExecutePackage();
                result = package.Execute();
                return Json(new {success= result, message="Executed Sussessfully"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = result, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult ExecutePackageB(PackageEntity att)
        {
            DataExtractor extractor = new DataExtractor();
            string sourceType = "dts";
            string sourceLocation = @"C:\Users\FINTRAK\Downloads\SSISPackage\SSISPackage\SCBDataHarmonizationSSIS\SCBDataHarmonizationSSIS\";
            string packageName = "Package";
            string res = extractor.LaunchPackage(sourceType, sourceLocation, packageName);
            return Json(res,JsonRequestBehavior.AllowGet);

        }
        public void SaveTransaction()
        {
            SaveLastLoadedData obj = new SaveLastLoadedData();
            obj.TransactionDate = DateTime.Now.ToString();
            entities.SaveLastLoadedDatas.Add(obj);
            entities.SaveChanges();
        }

        [HttpPost]
        public FileResult ExportSampleData()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[30] {
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
                                                     new DataColumn("Inflow"),
                                                      new DataColumn("Emtl"),
                                                      new DataColumn("ReceiverLocation")
            });

            var objException = from e in entities.tbl_SampleData select e;

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
                    item.Inflow,
                    item.Emtl,
                    item.ReceiverLocation
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
                        DETAIL = $"User {User.Identity.Name} exports transaction data successfully",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    entities.TBL_AUDIT.Add(audit);
                    entities.SaveChanges();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SampleData.xlsx");
                }
            }
        }

        public ActionResult UploadGuidLines()
        {
            return View(entities.LoadingGuidLines.ToList());
        }

        //Convert Index Page as PDF
        public ActionResult UploadGuides()
        {
            var report = new ActionAsPdf("UploadGuidLines");
            return report;
        }

    }
}