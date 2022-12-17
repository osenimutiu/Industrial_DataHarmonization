using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.Helper
{
    public class ExecutePackage
    {
        public bool Execute()
        {
            bool success = true;
            Application app = new Application();
            Package package = null;
            package = app.LoadPackage(@"C:\Users\FINTRAK\Downloads\SSISPackage\SSISPackage\SCBDataHarmonizationSSIS\SCBDataHarmonizationSSIS\Package.dtsx", null);
        //C: \Users\FINTRAK\Downloads\SSISPackage\SSISPackage\SCBDataHarmonizationSSIS\SCBDataHarmonizationSSIS\Package.dtsx
            package.Execute();
            return success;
        }
    }

    public class PackageEntity
    {
        public string exId { get; set; }
    }
}