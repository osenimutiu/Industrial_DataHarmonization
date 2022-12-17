using ExcelDataReader;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;


namespace SCBHarmonization.Helper
{
    public class CSVUpload
    {
        SCBDBEntities entities = new SCBDBEntities();


        //[ExceptionHandler]
        public List<tbl_RawHarmonizationDumpAlt> GetDataFromCSVFile(Stream stream, string FileNaming)
        {
            var empList = new List<tbl_RawHarmonizationDumpAlt>();
            //try
            //{
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // To set First Row As Column Names    
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                            empList.Add(new tbl_RawHarmonizationDumpAlt()
                            {
                                //Id = Convert.ToInt32(objDataRow["ID"].ToString()),
                                Amount = objDataRow["Amount"].ToString(),
                                Vat = objDataRow["Vat"].ToString(),
                                Fee = objDataRow["Fee"].ToString(),
                                TransID = objDataRow["TransID"].ToString(),
                                SrcAcctNo = objDataRow["SrcAcctNo"].ToString(),
                                SrcInstCode = objDataRow["SrcInstCode"].ToString(),
                                SrcInstBranchCode = objDataRow["SrcInstBranchCode"].ToString(),
                                SrcInstType = objDataRow["SrcInstType"].ToString(),
                                SrcInstUniqueID = objDataRow["SrcInstUniqueID"].ToString(),
                                //DestAccNo 
                                //DestAcctNo = objDataRow["DestAccNo"].ToString(),
                                DestAcctNo = objDataRow["DestAcctNo"].ToString(),
                                DestInstCode = objDataRow["DestInstCode"].ToString(),
                                DestInstBranchCode = objDataRow["DestInstBranchCode"].ToString(),
                                DestInstType = objDataRow["DestInstType"].ToString(),
                                DestInstUniqueID = objDataRow["DestInstUniqueID"].ToString(),
                                PaymentType = objDataRow["PaymentType"].ToString(),
                                BankIncome = objDataRow["BankIncome"].ToString(),
                                TransDate = objDataRow["TransDate"].ToString(),
                                PsspParty = objDataRow["PsspParty"].ToString(),
                                AccountType = objDataRow["AccountType"].ToString(),
                                AccountClass = objDataRow["AccountClass"].ToString(),
                                AccountDesignation = objDataRow["AccountDesignation"].ToString(),
                                Currency = objDataRow["Currency"].ToString(),
                                Channels = objDataRow["Channels"].ToString(),
                                TransactionTypeCode = objDataRow["TransactionTypeCode"].ToString(),
                                PepDesignatedAccount = objDataRow["PepDesignatedAccount"].ToString(),
                                //CypherSecurityLevyExempt = objDataRow["CypherSecurityLevyExempt"].ToString(),CypherSecurityLevyExempt
                                //CypherSecurityLevyExempt = objDataRow["CyberSecurityLevyExempt"].ToString(),
                                CypherSecurityLevyExempt = objDataRow["CypherSecurityLevyExempt"].ToString(),
                                
                                StampDutyExempt = objDataRow["StampDutyExempt"].ToString(),
                                AdditionalField = objDataRow["AdditionalField"].ToString(),
                                Inflow = objDataRow["Inflow"].ToString(),
                                Emtl = objDataRow["Emtl"].ToString(),
                                ReceiverLocation = objDataRow["ReceiverLocation"].ToString(),
                                FileDepartment = FileNaming
                                //Salary = Convert.ToInt32(objDataRow["Salary"].ToString()),
                            });
                        }
                        SaveTransaction();
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    string msg = ex.Message;
            //}
            return empList;
        }


        private void SaveTransaction()
        {
            SaveLastLoadedData obj = new SaveLastLoadedData();
            obj.TransactionDate = DateTime.Now.ToString();
            entities.SaveLastLoadedDatas.Add(obj);
            entities.SaveChanges();
        }

        public bool CopyFiles()
        {
            string fileName = "";
            //string sourcePath = @"C:\Users\FINTRAK\Desktop\SFTPFiles";
            //string sourcePath = @"C:\IT\SFTPFiles";
            string sourcePath = @"C:\csvFiles";
            string extension = "*.csv";
            string targetPath = @"C:\IDHProcessedFiles\IDH_";
            string destFile = String.Empty;
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            bool message = false;
            if (!System.IO.Directory.Exists(targetPath + Todaysdate))
            {
                System.IO.Directory.CreateDirectory(targetPath + Todaysdate);
            }
            if (System.IO.Directory.Exists(sourcePath))
            {
                //string[] files = System.IO.Directory.GetFiles(sourcePath);
                string[] files = System.IO.Directory.GetFiles(sourcePath, extension);
                foreach (string s in files)
                {
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath + Todaysdate, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    DeleteFiles(s);
                }
                message = true;
            }
            else
            {
                message = false;
            }
            return message;
        }

        public void DeleteFiles(string file)
        {
            System.IO.File.Delete(file);
        }

        public bool SaveAsCsv(string excelFilePath, string destinationCsvFilePath)
        {
            bool result = false;
            //string[] excelFilePath = Directory.GetFiles(@"C:\xlsxFile");
            string[] excelFiles = Directory.GetFiles(excelFilePath);
            //string destinationCsvFilePath = @"C:\Users\FINTRAK\Desktop\CSVFolder";
            string excelFilePathb = @"C:\xlsxFile";
            foreach (string s in excelFiles)
            {
                string excelFileName = Path.GetFileName(s);
                using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IExcelDataReader reader = null;
                    if (excelFileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (excelFileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    if (reader == null)
                    {
                        result = false;
                    }


                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });

                    var csvContent = string.Empty;
                    int row_no = 0;
                    while (row_no < ds.Tables[0].Rows.Count)
                    {
                        var arr = new List<string>();
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            arr.Add(ds.Tables[0].Rows[row_no][i].ToString());
                        }
                        row_no++;
                        csvContent += string.Join(",", arr) + "\n";
                    }
                    StreamWriter csv = new StreamWriter(destinationCsvFilePath, false);
                    csv.Write(csvContent);
                    csv.Close();
                    result = true;
                }
            }
            return result;
        }

        public void MoveFile(string sourceDirectory, string fileName)
        {
            string archiveDirectory = @"C:\IDHProcessedFiles\IDH_";
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            bool archiveExists = Directory.Exists(archiveDirectory + Todaysdate);
            if (!archiveExists)
            {
                Directory.CreateDirectory(archiveDirectory + Todaysdate);
            }
            Directory.Move(sourceDirectory, Path.Combine(archiveDirectory + Todaysdate, fileName));
        }
    }
    public static class Extension
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties  
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table   
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows  
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

    }
}