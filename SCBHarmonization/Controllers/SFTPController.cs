using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinSCP;

namespace SCBHarmonization.Controllers
{
    public class SFTPController : Controller
    {
        // GET: SFTP
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(SFTPViewModel obj)
        {

            string message = "";
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = obj.HostName,
                    UserName = obj.UserName,
                    //Password = obj.Password,
                    //SshHostKeyFingerprint = "ssh-rsa 2048 xxxxxxxxxxx..."
                    SshHostKeyFingerprint = "ssh-rsa 2048 " + obj.SshHostKeyFingerprint,
                    //SshHostKeyFingerprint = obj.SshHostKeyFingerprint,
                    PortNumber = obj.Port
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult =
                        //session.PutFiles(@"d:\ForVideos\*.xml", "/public/", false, transferOptions);
                        session.GetFiles(@"/Datah/", @"C:\Users\gauta\Downloads\RebexTinyStfpServerBin-1.0.8\local\*.xlsx", false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        //Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        Console.WriteLine("Download of {0} succeeded", transfer.FileName);
                        message = "Downloaded Successfully";
                    }
                }

                //return 0;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //return 1;
                message = "An error encountered please check after sometimes";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }


    }
}