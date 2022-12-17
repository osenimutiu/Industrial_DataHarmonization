using Microsoft.Extensions.DependencyInjection;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Models;
using SCBHarmonization.NibssClass;
using SCBHarmonization.NibssModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SCBHarmonization.LicenseValidator;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        HttpClientHelper modelbuilder1 = new HttpClientHelper();

        private SCBDBEntities db;
        public HomeController()
        {
            db = new SCBDBEntities();

        }
        public ActionResult Index()
        {
            //var dashboardItem = from e in db.tbl_Dashboad select e;
            //return View(dashboardItem);
            //var dashboardItem = from e in db.tbl_TopFive_ResponseSummary select e;

            //string Date = 
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
            cmd.CommandText = "sp_transaction_summary";
            cnn.Open();
            object o = cmd.ExecuteScalar();
            var dashboardItem = db.tbl_TopFive_ResponseSummary.OrderByDescending(x=>x.Id).Take(5).ToList();
            //LicenseValidator licensevalidator = new LicenseValidator();
            //if (licensevalidator.ExamineLicense().Status == LicenseStatus.ValidLicense)
            //{
            //    ViewBag.Message = "License is Valid";
            //}
            //using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //using (var command = new SqlCommand("sp_ResetDateCountdown", conn)
            //{
            //    CommandType = CommandType.StoredProcedure

            //})
            //{
            //    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //    returnParameter.Direction = ParameterDirection.ReturnValue;
            //    conn.Open();
            //    command.ExecuteNonQuery();
            //    var result = returnParameter.Value;
            //    ViewBag.Days = result;
            //}
            //using (SCBDBEntities  context = new SCBDBEntities()
            //{
            //    context.Configuration.
            //}
            return View(dashboardItem);

        }

        [AllowAnonymous]
        public ActionResult LicenseError()
        {
            ViewBag.Title = "License Error Page";
            LicenseValidator licensevalidator = new LicenseValidator();
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.MissingLicenseFile)
            {
                ViewBag.Message = "Your License is Missing";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.CorruptLicenseFile)
            {
                ViewBag.Message = "Your License is Currupted";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.InvalidSignature)
            {
                ViewBag.Message = "Your License Signature is Invalid";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.NotYetLicensed)
            {
                ViewBag.Message = "You are not yet Licensed";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.LicenseExpired)
            {
                ViewBag.Message = "Your License has Expired";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseStatus.VersionMismatch)
            {
                ViewBag.Message = "Your License Version is Invalid";
            }
            if (licensevalidator.ExamineLicense().Status == LicenseValidator.LicenseStatus.WrongLicensee)
            {
                ViewBag.Message = String.Format("Your Company is not Licensed to use this Product");
            }
            if (licensevalidator.ExamineLicense().Status == LicenseValidator.LicenseStatus.WrongProductLicense)
            {
                ViewBag.Message = String.Format("You are using a Wrong License for this Product");
            }
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SendTypedTransaction()
        {
            ViewBag.Message = "Send Transaction";

            return View();
        }
        public ActionResult LastFiveResponse(string name)
        {

            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_response";
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            ViewBag.Message = string.Format("Sent response updated Successfully {0}", name, DateTime.Now.ToString());
            LastFiveCount(name);
            return RedirectToAction("Index");
        }

        public ActionResult LastFiveCount(string name)
        {

            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_insert_from_responsesummary";
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            ViewBag.Message = string.Format("Sent response updated Successfully {0}", name, DateTime.Now.ToString());
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> SendTypedTransaction(TransactionRequest transactionRequest)
        {
            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();
                Credential credential = db.Credentials.Find(1);

                string username = credential.USERNAME;
                string iv = credential.IV;
                string key = credential.KEY;

                var SendDataResponse = await dataAggregationServices.SendTransactionDataAsync(transactionRequest, (iv: iv, key: key, username: username));
                ViewBag.SendDataResponse = SendDataResponse;
                ViewData["success"] = SendDataResponse;
                TransactionStatusController savestatus = new TransactionStatusController();
                TransactionStatus transactionStatus = new TransactionStatus();
                transactionStatus.Message = SendDataResponse.Message;
                transactionStatus.Status = SendDataResponse.Status;
                transactionStatus.Description = SendDataResponse.Description;
                transactionStatus.Data = SendDataResponse.Data;
                transactionStatus.SendDate = DateTime.Now;
                savestatus.Create(transactionStatus);
                ViewBag.Status = SendDataResponse.Status;
                ViewBag.Message = SendDataResponse.Message;
                ViewBag.Description = SendDataResponse.Description;
                ViewBag.Data = SendDataResponse.Data;
                return View();
            }
        }

        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("application/octet-stream");
        }

        private bool isValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; //1MB
        }

        [HttpPost]
        public ActionResult Process(HttpPostedFileBase LicenseFile)
        {
            if (!isValidContentType(LicenseFile.ContentType))
            {
                ViewBag.Error = "Only License Files is allowed";
                return View("LicenseError");
            }
            else if (!isValidContentLength(LicenseFile.ContentLength))
            {
                ViewBag.Error = "Your File size is too large";
                return View("LicenseError");
            }
            else
            {
                if (LicenseFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(LicenseFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/License"), fileName);
                    string deletepath = Server.MapPath("License/FinTrakIFRSLicense.lic");
                    if (System.IO.File.Exists(deletepath))
                    {
                        System.IO.File.Delete(deletepath);
                    }
                    LicenseFile.SaveAs(path);
                }
            }
            return RedirectToAction("Login", "Account");
        }



        public string curAssPro()
        {
            System.Reflection.Assembly currentAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (currentAssembly == null) currentAssembly = System.Reflection.Assembly.GetCallingAssembly();
            string product = currentAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            return product;
        }

        public string curAssCom()
        {
            System.Reflection.Assembly currentAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (currentAssembly == null) currentAssembly = System.Reflection.Assembly.GetCallingAssembly();
            string company = currentAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            return company;
        }

        [AllowAnonymous]
        [HttpPost]
        public void SendMail(mailStruct maildata)
        {
            string startdate = maildata.StartDate;
            int year = maildata.Year;
            string errormsg = maildata.Errormessage;
            string comment = maildata.Comment;
            //do stuff
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //,abiodun.oyekunle@fintraksoftware.com
                mail.From = new MailAddress("pi360license@gmail.com");
                mail.To.Add("ikennabenedict@gmail.com,ejiolawale4@gmail.com,abylibrary@gmail.com");
                mail.Subject = curAssCom() + "License Request";
                mail.Body = "Dear FinTrak Team," +
                    "\n\nKindly take this as a formal request for a license of " + year + " year(s) on "
                    + curAssPro() + " product starting from " + startdate +
                    "\n\nRegards," +
                    "\n" + curAssCom();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("pi360license@gmail.com", "pi360support");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("mail Send");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public ActionResult Encrypt()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Encrypt(PersonModel personModel)
        {
            string name = "DefaultConnection";
            bool isNew = false;
            string path = Server.MapPath("~/Web.Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
            XmlNode node;
            isNew = list.Count == 0;
            if (isNew)
            {
                node = doc.CreateNode(XmlNodeType.Element, "add", null);
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.Value = name;
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("connectionString");
                attribute.Value = "";
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("providerName");
                attribute.Value = "System.Data.SqlClient";
                node.Attributes.Append(attribute);
            }
            else
            {
                node = list[0];
            }
            string conString = node.Attributes["connectionString"].Value;
            SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
            conStringBuilder.InitialCatalog = personModel.Database;
            conStringBuilder.DataSource = personModel.Server;
            conStringBuilder.IntegratedSecurity = false;
            conStringBuilder.UserID = personModel.User;
            conStringBuilder.Password = personModel.Password;
            node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
            if (isNew)
            {
                doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
            }
            doc.Save(path);
            return View();
        }
        public static void EncryptConnString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }

    }
}