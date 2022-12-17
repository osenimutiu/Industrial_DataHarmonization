using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace SCBHarmonization.Helper
{
    public class SendEmail
    {
        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        public static bool EmailSend(string SenderEmail, string Subject, string Message, bool IsBodyHtml = false)
        {

            bool status = false;
            try
            {
                string HostAddress = ConfigurationManager.AppSettings["Host"].ToString();
                string FormEmailId = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                string Port = ConfigurationManager.AppSettings["Port"].ToString();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FormEmailId);
                mailMessage.Subject = Subject;
                mailMessage.Body = Message;
                mailMessage.IsBodyHtml = IsBodyHtml;
                mailMessage.To.Add(new MailAddress(SenderEmail));

                SmtpClient smtp = new SmtpClient();
                smtp.Host = HostAddress;

                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = mailMessage.From.Address;
                networkCredential.Password = Password;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                smtp.Port = Convert.ToInt32(Port);

                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception ex)
                {

                    throw;
                }

                status = true;
                return status;
            }
            catch (Exception e)
            {
                return status;
            }
        }

        public static string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address.ToString();
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }
        public static string GetCurrentWebsiteRoot()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }

        public static string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }
        public static string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public static string GetDeviceName()
        {

            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["Manufacturer"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }

        }
    }
}