using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using Renci.SshNet;
using SCBHarmonization.ViewModels;

namespace SCBHarmonization.Helper
{
    public class ExcelToCSV
    {
        public string ToCSV()
        {
            string result = "";
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            String fromFile = @"C:\xlsxFile";
            //String toFile = @"C:\Users\FINTRAK\Desktop\CSVFolder";
            String toFile = @"C:\csvFiles";

            //File to delete para
            //string fileToDelete = @"C:\Users\FINTRAK\Desktop\CSVFolder";
            //string[] allFilesToDelete = Directory.GetFiles(fileToDelete);
            //foreach (String file in allFilesToDelete)
            //{
            //    File.Delete(file);
            //}

            if (!Directory.Exists(toFile))
            {
                Directory.CreateDirectory(toFile);
            }
            string[] excelFiles = Directory.GetFiles(fromFile, "*.xlsx");
            foreach (string s in excelFiles)
            {
                string excelFileName = Path.GetFileName(s);
                string extension = ".csv";
                string realfile = Path.GetFileNameWithoutExtension(s);
                string filenamedest = System.IO.Path.Combine(toFile, realfile + extension);
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(s, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // this does not throw exception if file doesnt exist
                File.Delete(filenamedest);
                wb.SaveAs(filenamedest, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSVWindows, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, false, Type.Missing, Type.Missing, Type.Missing);

                wb.Close(false, Type.Missing, Type.Missing);
            }
            app.Quit();
            result = excelFiles.Length + " files converted to csv succesfully";
            return result;
        }

        public void ToCSVb(string fileToConvert)
        {
            string result = "";
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            //String fromFile = @"C:\xlsxFile";
            //String toFile = @"C:\csvFiles";
            String toFile = @"C:\csvFilesAlt";

            
            if (!Directory.Exists(toFile))
            {
                Directory.CreateDirectory(toFile);
            }
                string excelFileName = Path.GetFileName(fileToConvert);
                string extension = ".csv";
                string realfile = Path.GetFileNameWithoutExtension(fileToConvert);
                string filenamedest = System.IO.Path.Combine(toFile, realfile + extension);
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(fileToConvert, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                File.Delete(filenamedest);
                wb.SaveAs(filenamedest, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSVWindows, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, false, Type.Missing, Type.Missing, Type.Missing);

                wb.Close(false, Type.Missing, Type.Missing);
            app.Quit();
        }  


        public bool DownloadAll(SFTPViemModelB obj)
        {
            //string date = obj.DownloadDate;
            DateTime dateTime = DateTime.Parse(obj.DownloadDate).Date;
            string date = dateTime.ToString();
            string host = "192.168.184.118";
            string username = "Fintrak";
            string password = "P@ssword10$";
            DateTime dateToDownload = DateTime.Now;
            bool result = false;
            //string remoteDirectory = "/";
            //string remoteDirectory = "/" + date + "/";
            string remoteDirectory = "/datah/";
            string moveToDirectory = "/dataMove/";
            string destination = "/sftpDestination/";
            string localDirectory = @"C:\Users\FINTRAK\Desktop\CSVFolder\";

            using (var sftp = new SftpClient(host, username, password))
            {
                sftp.Connect();
                //if (sftp.Exists(remoteDirectory))
                //{
                //    var filesi = sftp.ListDirectory(remoteDirectory);

                //}
                //sftp.ChangeDirectory(remoteDirectory);
                var files = sftp.ListDirectory(remoteDirectory);
                //DateTime[] creationtime = new DateTime[files.Count()];
                foreach (var file in files)
                {

                   
                    
                    //ToCSVb(file.Name);
                    string remoteFileName = file.Name;
                    //if ((!file.Name.StartsWith(".")) && ((file.LastWriteTime.Date == DateTime.Today)))
                    if ((!file.Name.StartsWith(".")) && ((file.LastWriteTime.Date == dateToDownload.Date)))
                        //if ((!file.Name.StartsWith(".")) && ((file.GetCreateDt == DateTime.Today)))

                        using (Stream file1 = File.Create(localDirectory + remoteFileName))
                        {
                            //var inFile = sftp.Get(remoteDirectory);
                            //inFile.MoveTo(moveToDirectory);

                            
                            sftp.DownloadFile(remoteDirectory + remoteFileName, file1);
                            //When you want to move file and place in try catch block.
                            //file.MoveTo(destination + file.Name);
                        }
                }
                result = true;

            }
            return result;
        }

        public string ExportToCSV()
        {
            string result = String.Empty;
            string DownloadPath = @"C:\IDH\";
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string fullPath = Path.Combine(DownloadPath,Todaysdate);
            bool isFileExist = Directory.Exists(fullPath);
            if (!isFileExist)
            {
                Directory.CreateDirectory(fullPath);
            }
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            sqlCon.Open();
            //string query = "select * from tbl_PreHarmonization where TransID in (select top 600000 Data from Tbl_SendData where status != '00')";
            string query = "select Amount,Vat,Fee,TransID,SrcAcctNo,SrcInstCode,SrcInstBranchCode,SrcInstType,SrcInstUniqueID,DestAcctNo as DestAccNo,DestInstCode,DestInstBranchCode,DestInstType,DestInstUniqueID,PaymentType,BankIncome,TransDate,PsspParty,AccountType,AccountClass,AccountDesignation,Currency,Channels,TransactionTypeCode,PepDesignatedAccount ,CypherSecurityLevyExempt as CyberSecurityLevyExempt,StampDutyExempt,AdditionalField,Inflow,Emtl,ReceiverLocation from tbl_PreHarmonization where TransID in (select top 600000 Data from Tbl_SendData where status != '00' and (SELECT CAST(SentDate AS DATE))=(select CAST(GETDATE() As Date)) order by id desc)";
            SqlCommand sqlCmd = new SqlCommand(
                query, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            string fileName = "NIBSS_Exception.csv";
            string path = System.IO.Path.Combine(fullPath, fileName);
            //StreamWriter sw = new StreamWriter(fileName);
            StreamWriter sw = new StreamWriter(path);
            object[] output = new object[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                output[i] = reader.GetName(i);
            sw.WriteLine(string.Join(",", output));
            while (reader.Read())
            {
                reader.GetValues(output);
                sw.WriteLine(string.Join(",", output));
            }
            sw.Close();
            reader.Close();
            sqlCon.Close();
            result = "File downloaded to path " + fullPath;
            return result;
        }

    }
}