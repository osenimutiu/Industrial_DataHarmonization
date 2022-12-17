using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Xml;
using static SCBHarmonization.LicenseValidator;

namespace SCBHarmonization
{
    public partial class SMPTMail
    {
        public void button1_Click()
        {
            try
            {
                string usePath = @"C:\Users\JARUMIEB\source\repos\SterlApplication\SterlApplication\Application\Presentation\Fintrak.Presentation.WebClient\License\FinTrakIFRSLicense.lic";
                //usePath = @"C:\inetpub\wwwroot\AccessClient\License\FinTrakIFRSLicense.lic";
                if (string.IsNullOrEmpty(usePath))
                {
                    usePath = System.AppDomain.CurrentDomain.BaseDirectory + "/License/FinTrakIFRSLicense.lic";
                }
                   
                var result = new LicenseFileDetail();
                XmlNodeList matchingNodes;
                XmlDocument licenseContent;
                

                licenseContent = new XmlDocument();
                licenseContent.Load(usePath);
                matchingNodes = licenseContent.GetElementsByTagName("LicenseDate");
                result.ExpireDate = Conversions.ToDate(matchingNodes[0].InnerText);


                //,abylibrary@gmail.com
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("pi360license@gmail.com");
                mail.To.Add("ikennabenedict@gmail.com,ejiolawale4@gmail.com,abylibrary@gmail.com");
                mail.Subject = "PI360 LICENSE ABOUT TO EXPIRE" + result.ExpireDate.Subtract(DateTime.Today);
                mail.Body = "Dear " + result.Licensee +
                    "This is to remind you that your "+ result.Product + " Application \n license will in " + result.ExpireDate.Subtract(DateTime.Today) +" days,\n " +
                    "kindly reach out to Fintrak team to avoid been locked out of the system" +
                    "\n\n Regards \nFintrak Licence Engine";

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
    }
}