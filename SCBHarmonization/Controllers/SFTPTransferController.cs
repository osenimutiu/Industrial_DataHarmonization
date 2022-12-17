using Microsoft.AspNet.Identity;
using Renci.SshNet;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    public class SFTPTransferController : Controller
    {
        private readonly SCBDBEntities objDbEntities;
        public SFTPTransferController()
        {
            objDbEntities = new SCBDBEntities();
        }
        // GET: SFTPTransfer
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public JsonResult Index(SFTPViewModel obj)
        //{

        //    RandomPassword randpass = new RandomPassword();
        //    string message = randpass.SfptDownload();

        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult Index(SFTPViewModel obj)
        {
            RandomPassword randpass = new RandomPassword();
            string message = randpass.SfptDownload();
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"SFTP files downloaded successfully by {User.Identity.Name}",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadFileWithDate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Download(SFTPViemModelB obj)
        {
            string message = String.Empty;
            try
            {
                ExcelToCSV repo = new ExcelToCSV();
                bool response = repo.DownloadAll(obj);
                if (response)
                {
                    message = "Download Successfully"; 
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"SFTP files downloaded successfully by {User.Identity.Name}",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                    objDbEntities.SaveChanges();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ExportNIBSSException(SFTPViemModelB obj)
        {
            string message = String.Empty;
            try
            {
                ExcelToCSV repo = new ExcelToCSV();
                string response = repo.ExportToCSV();
                //if (response)
                //{
                    message = "Export Successfully";
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri,
                        UserName = User.Identity.Name,
                        DETAIL = $"SFTP files downloaded successfully by {User.Identity.Name}",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                    objDbEntities.TBL_AUDIT.Add(audit);
                    objDbEntities.SaveChanges();
                    return Json(response, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }


        //public JsonResult Indexb(SFTPViewModel obj)
        //{
        //    string message = "";
        //    RandomPassword randpass = new RandomPassword();
        //    try
        //    {
        //        bool resp = randpass.DownloadSftpFiles(obj);
        //        if (resp)
        //        {
        //            message = "Download Successfully";
        //            return Json(message, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(e.Message, JsonRequestBehavior.AllowGet);
        //        throw;
        //    }

        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Index(SFTPViewModel obj)
        //{
        //    //using (SftpClient sftpClient = new SftpClient(getSftpConnection(obj.HostName, obj.UserName, obj.Port, obj.PublicKeyPath)))
        //    //{
        //    //    sftpClient.Connect();
        //    //    using (FileStream fs = new FileStream("filePath", FileMode.Open))
        //    //    {
        //    //        sftpClient.BufferSize = 1024;
        //    //        sftpClient.UploadFile(fs, Path.GetFileName("filePath"));
        //    //    }
        //    //    sftpClient.Dispose();
        //    //}
        //    string message = "";
        //    obj.HostName = "192.168.56.118";
        //    obj.UserName = "Fintrak";
        //    obj.Port = 22;
        //    try
        //    {
        //        //using (SftpClient sftpClient = new SftpClient(getSftpConnection(obj.HostName, obj.UserName, obj.Port, obj.PublicKeyPath)))
        //        using (SftpClient sftpClient = new SftpClient(getSftpConnection(obj.HostName, obj.UserName, obj.Port)))
        //        {
        //            sftpClient.Connect();

        //            //string serverPath = @"/Datah";
        //            string serverPath = @"C:\Users\FINTRAK\Desktop\SFTPFiles";
        //            string localPath = @"C:\Users\FINTRAK\Documents\SFTPFile";
        //            using (FileStream fs = new FileStream(localPath, FileMode.Open))
        //            {
        //                sftpClient.BufferSize = 1024;
        //                sftpClient.DownloadFile(serverPath, fs, x => Console.WriteLine(x));
        //            }
        //            message = "Downloaded Successfully";
        //            sftpClient.Dispose();
        //        }
        //        var audit = new TBL_AUDIT()
        //        {
        //            URL = Request.Url.AbsoluteUri,
        //            UserName = User.Identity.Name,
        //            DETAIL = $"File downloaded by '{User.Identity.Name}'",
        //            IPADDRESS = SendEmail.GetLocalIpAddress(),
        //            DEVICENAME = SendEmail.GetDeviceName(),
        //            OSNAME = SendEmail.FriendlyName(),
        //            DATETIME = DateTime.UtcNow,
        //            UserId = acc.User.Identity.GetUserId()
        //        };
        //        return Json(message, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {

        //        message = "An error encountered please check after sometimes";
        //        return Json(message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public static ConnectionInfo getSftpConnection(string host, string username, int port, string publicKeyPath)

        public static ConnectionInfo getSftpConnection(string host, string username, int port)
        {
            //return new ConnectionInfo(host, port, username, privateKeyObject(username, publicKeyPath));
            return new ConnectionInfo(host, port, username, privateKeyObject(username, ""));
        }

        private static AuthenticationMethod[] privateKeyObject(string username, string publicKeyPath)
        {
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(publicKeyPath);
            PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            return new AuthenticationMethod[] { privateKeyAuthenticationMethod };
        }

        
    }
}