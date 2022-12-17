using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.VisualBasic;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace SCBHarmonization.Helper
{
    public class DataExtractor
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public string LaunchPackage(string sourceType, string sourceLocation, string packageName) // DTSExecResult
        {
            string msg;
            try
            {
                string packagePath;
                Package myPackage;
                Application integrationServices = new Application();
                string Script;
                // Combine path and file name.
                packagePath = Path.Combine(sourceLocation, packageName);
                switch (sourceType)
                {
                    case "dts":
                        {
                            // Package is managed by SSIS Package Store.
                            // Default logical paths are File System and MSDB.
                            // If integrationServices.ExistsOnDtsServer(packageName, "sourceLocation") Then
                            // myPackage = integrationServices.LoadFromDtsServer(packagePath, "localhost")
                            myPackage = integrationServices.LoadPackage(packagePath + ".dtsx", null/* TODO Change to default(_) if this is not a reference type */);
                            //Script = myPackage.Variables("StorProc").Value;
                            //Script = Script.Replace("@StartDate", StartDate); // .Replace("@StartDate", EndDate)
                            //myPackage.Variables("StorProc").Value = Script;
                            break;
                        }

                    default:
                        {
                            throw new ApplicationException("Invalid sourceType argument : valid values are 'file', 'sql', and 'dts'.");
                            break;
                        }
                }
                DTSExecResult SSIS_PackageResults;
                int _ssisCount = 0;
                myPackage.MaximumErrorCount = 100;
                myPackage.FailPackageOnFailure = false;
                myPackage.FailParentOnFailure = false;
                myPackage.DelayValidation = true;
                SSIS_PackageResults = myPackage.Execute();
                foreach (DtsError pkgError in myPackage.Errors)
                {
                    Console.WriteLine();
                    Console.WriteLine("Description  {0}", pkgError.Description);
                    Console.WriteLine("HelpContext  {0}", pkgError.HelpContext);
                    Console.WriteLine("HelpFile     {0}", pkgError.HelpFile);
                    Console.WriteLine("IDOfInterfaceWithError {0}", pkgError.IDOfInterfaceWithError);
                    Console.WriteLine("Source       {0}", pkgError.Source);
                    Console.WriteLine("Subcomponent {0}", pkgError.SubComponent);
                    Console.WriteLine("Timestamp    {0}", pkgError.TimeStamp);
                    Console.WriteLine("ErrorCode    {0}", pkgError.ErrorCode);
                }


                msg = "Records Successfully Extracted for " + packageName + "";
                // ' Dim SSISExt As New DataSet


                if (packageName == "DataLoader")
                    //_ssisCount = SqlHelper.ExecuteScalar(AppConfig.ConnectionString, "proc_countLoadedData");
                    _ssisCount = (int)SqlHelper.ExecuteScalar(con.ConnectionString, "proc_countLoadedData");
                // ' _ssisCount = myPackage.Variables("RowCount").Value
                msg = "Records Extracted for " + packageName + " : " + Strings.Format(_ssisCount, "###,##,#");
                return msg;
            }
            catch (Exception ex)
            {
                // Return "Failed"
                //return msg;
                return ex.Message;
            }
        }
    }
}