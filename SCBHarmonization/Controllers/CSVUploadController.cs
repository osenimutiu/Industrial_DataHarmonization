//using SCBDataHarmonizationApi.Models;
using Microsoft.AspNet.Identity;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    
    public class CSVUploadController : Controller
    {
        private readonly SCBDBEntities objEntities;
        ExcelToCSV uploadb = new ExcelToCSV();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public CSVUploadController()
        {
            objEntities = new SCBDBEntities();
        }
        // GET: CSVUpload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ImportFile(HttpPostedFileBase importFile)
        {
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_RawHarmonizationDumpAlt]");
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_FirstHarmonizationDump] ");
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_PreHarmonization]");
            CSVUpload handler = new CSVUpload();
            if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });
            string FileNaming = importFile.FileName.Substring(0, 4);
            try
            {
                var fileData = handler.GetDataFromCSVFile(importFile.InputStream, FileNaming);
                var dtEmployee = fileData.ToDataTable();
                
                
                var tblEmployeeParameter = new SqlParameter("tblHarmonizationTableType", SqlDbType.Structured)
                {
                    TypeName = "dbo.tblTypeHarmonization",
                    Value = dtEmployee
                };
                await objEntities.Database.ExecuteSqlCommandAsync("EXEC spBulkImportHarmonization @tblHarmonizationTableType", tblEmployeeParameter);
                //Modified
                SqlCommand cmdd = new SqlCommand();
                cmdd.Connection = con;
                cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                cmdd.CommandText = "RawCSVExcel_DataTransfer";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "CSVExcel_DataTransfer";
                con.Open();
                object o = cmdd.ExecuteScalar();
                object r = cmd.ExecuteScalar();
                con.Close();
                //FileMigration fileMigration = new FileMigration();
                //string result = fileMigration.SampleFile(importFile.FileName);
                //if (result != "Moved")
                //{
                //    return Json(new { Status = 0, Message = result });
                //}
                //Modified End
                var audit = new TBL_AUDIT()
                {
                    URL = Request.Url.AbsoluteUri,
                    UserName = User.Identity.Name,
                    DETAIL = $"{importFile.FileName} upload by {User.Identity.Name} is successful",
                    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    DEVICENAME = SendEmail.GetDeviceName(),
                    OSNAME = SendEmail.FriendlyName(),
                    DATETIME = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId()
                };
                objEntities.TBL_AUDIT.Add(audit);
                objEntities.SaveChanges();
                return Json(new { Status = 1, Message = "File Imported Successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { Status = 2, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ImportMultipleFiles(HttpPostedFileBase importFile)
        {
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_RawHarmonizationDumpAlt]");
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_FirstHarmonizationDump] ");
            objEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE [tbl_PreHarmonization]");
            CSVUpload handler = new CSVUpload();
            //if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });
            var fileData = new List<tbl_RawHarmonizationDumpAlt>();
            try
            {
                //@"C:\csvFiles"
                //var filePaths = Directory.GetFiles(@"C:\IT\SFTPFiles", "*.csv");
                //var filePaths = Directory.GetFiles(@"C:\csvFiles", "*.csv");
                var filePaths = Directory.EnumerateFiles(@"C:\csvFiles", "*.csv", SearchOption.AllDirectories);
                //var filePaths = Directory.EnumerateFiles(@"C:\csvFiles", "*.xlsx", SearchOption.AllDirectories);
                var sourceDirectory = @"C:\csvFiles";
                int fileCount = filePaths.Count();
                if (fileCount == 0)
                {
                    return Json(new { Status = 3, Message = "No file selected" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    foreach (string s in filePaths)
                    {
                        
                        string fileName = s.Substring(sourceDirectory.Length + 1);
                        string FileNaming = fileName.Substring(0, 4);
                        using (StreamReader sr = new StreamReader(s))
                        {
                            fileData = handler.GetDataFromCSVFile(sr.BaseStream, FileNaming);
                            var dtEmployee = fileData.ToDataTable();
                            var tblEmployeeParameter = new SqlParameter("tblHarmonizationTableType", SqlDbType.Structured)
                            {
                                TypeName = "dbo.tblTypeHarmonization",
                                Value = dtEmployee
                            };
                            await objEntities.Database.ExecuteSqlCommandAsync("EXEC spBulkImportHarmonization @tblHarmonizationTableType", tblEmployeeParameter);
                            handler.MoveFile(s, fileName);
                        }
                    }

                    //Modified
                    SqlCommand cmdd = new SqlCommand();
                    cmdd.Connection = con;
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.CommandText = "RawCSVExcel_DataTransfer";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CSVExcel_DataTransfer";
                    con.Open();
                    object o = cmdd.ExecuteScalar();
                    object r = cmd.ExecuteScalar();
                    con.Close();
                    //bool fileMove = handler.CopyFiles();
                    //if (fileMove)
                    //{
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"{fileCount} files uploaded by {User.Identity.Name} are successful",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objEntities.TBL_AUDIT.Add(audit);
                    objEntities.SaveChanges();
                    return Json(new { Status = 1, Message = fileCount + " Files Imported Successfully" }, JsonRequestBehavior.AllowGet);

                    //}
                    //return Json(new { Status = 4, Message = "Umable to move files" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLogger logger = new ExceptionLogger
                {
                    ExceptionMessageWithSolution = ex.StackTrace,
                    ExceptionStackTrace = ex.StackTrace,
                    ControllerName = "CSVUpload",
                    ActionName = "ImportFile",
                    InnerException = ex.Message,
                    LogTime = DateTime.Now
                };
                objEntities.ExceptionLoggers.Add(logger);
                objEntities.SaveChanges();
                return Json(new { Status = 2, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ConvertToCSV(SFTPViewModel obj)
        {
            CSVUpload upload = new CSVUpload();
            string message = String.Empty;
            string excelFilePath = @"C:\xlsxFile";
            string destinationCsvFilePath = @"C:\Users\FINTRAK\Desktop\CSVFolder";
            bool status = false;
            try
            {
                //bool resp = upload.SaveAsCsv(excelFilePath, destinationCsvFilePath);
                string resp = uploadb.ToCSV();
                //if (resp)
                //{
                    status = true;
                    message = resp;
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult IsFileEmpty()
        {
            bool IsFileEmpty = false;
            var filePaths = Directory.EnumerateFiles(@"C:\csvFiles", "*.csv", SearchOption.AllDirectories);
            string fPath = @"C:\csvFiles";
            string msg = String.Empty;
            int fileCount = filePaths.Count();
            if (fileCount == 0)
            {
                IsFileEmpty = true;
                msg = "No csv file available in path " + fPath + ", kindly convert to csv files prior to upload";
                return Json(new { fileEmpty = IsFileEmpty, Message = msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                IsFileEmpty = false;
                msg = fileCount + " csv files available in path " + fPath + ", Kindly upload";
                return Json(new { fileEmpty = IsFileEmpty, Message = msg }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { fileEmpty = IsFileEmpty, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}