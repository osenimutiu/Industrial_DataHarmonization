using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.Helper
{
    public class FileMigration
    {
        public bool MoveFile(string fileName)
        {
            bool resp = false;
            string sourcePath = @"C:\IT\SFTPFiles";
            string targetPath = @"C:\IT\SFTPFiles\destination";
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach(string s in files)
                {
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile,true);
                }
                resp = true;
            }
            else
            {
                resp = false;
            }
            return resp;
        }

        public string SampleFile(string fileName)
        {
            string resp = String.Empty;
            try
            {
                string sourcePath = @"C:\IT\SFTPFiles";
                string targetPath = @"C:\IT\SFTPFiles\destination";
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destinationFile = System.IO.Path.Combine(targetPath, fileName);
                //To Move File
                System.IO.File.Move(sourceFile, destinationFile);
                resp = "Moved";
                return resp;
                //To Move Entire Directory
                //System.IO.Directory.Move(@"C:\IT\SFTPFiles", @"C:\IT\SFTPFiles\destination");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            

        }
    }
}