using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Reflection;
using DocumentFormat.OpenXml.CustomProperties;

namespace SCBHarmonization 
{
    internal partial class LicenseValidator
    {
        // ----- Public constants.
        public const string ProgramTitle = "The FinTrak License Project";
        public const string NotAuthorizedMessage = "You are not authorized to perform this task.";
        public const int UseDBVersion = 1;
        public const string DefaultLicenseFile = "FinTrakIFRSLicense.lic";

        // ----- Constants for the MatchingImages image list.
        public const int MatchPresent = 0;
        public const int MatchNone = 1;
        public static DateTime currentdate;


        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);
        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }

        public static DateTime InternetDateTime()
        {
            if (CheckNet() == false)
            {
                var client = new TcpClient("time.nist.gov", 13);
                try
                {
                    using (var streamReader = new StreamReader(client.GetStream()))
                    {
                        var response = streamReader.ReadToEnd();
                        var utcDateTimeString = response.Substring(7, 17);
                        var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                        return localDateTime;
                    }
                }
                catch
                {
                    return new DateTime();
                }
            }
            else
            {
              return DateTime.Today;
            }
            
        }


        public enum LicenseStatus
        {
            ValidLicense,
            MissingLicenseFile,
            CorruptLicenseFile,
            InvalidSignature,
            NotYetLicensed,
            LicenseExpired,
            VersionMismatch,
            WrongLicensee,
            WrongProductLicense
        }

        public partial struct LicenseFileDetail
        {
            public LicenseStatus Status;
            public string Licensee;
            public DateTime LicenseDate;
            public DateTime ExpireDate;
            public string CoveredVersion;
            public string ServrMAC;
            public string Product;
            public string ServerMotherBoard;

        }

        public static string EncryptPassword(string loginID, string passwordText)
        {
            // ----- Given a username and a password, encrypt the password so that it
            // is not easy to decrypt. There is no limit on password length since
            // it is going to be hashed anyway.
            HMACSHA1 hashingFunction;
            byte[] secretKey;
            byte[] hashValue;
            int counter;
            string result = "";

            // ----- Prepare the secret key. Force it to uppercase for consistency,
            // and then stuff it in a byte array.
            secretKey = new UnicodeEncoding().GetBytes(Strings.UCase(loginID));

            // ----- Create the hashing component using Managed SHA-1.
            hashingFunction = new HMACSHA1(secretKey, true);

            // ----- Calculate the hash value. One simple line of code.
            hashValue = hashingFunction.ComputeHash(new UnicodeEncoding().GetBytes(passwordText));

            // ----- The hash value is ready, but I like things in plain text when
            // possible. Let's convert it to a long hex string.
            var loopTo = hashValue.Length - 1;
            for (counter = 0; counter <= loopTo; counter++)
                result += Conversion.Hex(hashValue[counter]);

            // ----- Stored passwords are limited to 20 characters.
            return Strings.Left(result, 20);
        }

        public LicenseFileDetail ExamineLicense()
        {
            // ----- Examine the application's license file, and report back
            // what's inside.
            var result = new LicenseFileDetail();
            string usePath;
            XmlDocument licenseContent;
            RSA publicKey;
            SignedXml signedDocument;
            XmlNodeList matchingNodes;
            string[] versionParts;
            int counter;
            string comparePart;

            System.Reflection.Assembly currentAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (currentAssembly == null) currentAssembly = System.Reflection.Assembly.GetCallingAssembly();
            string directorypath = System.IO.Path.GetDirectoryName(currentAssembly.Location);

            // ----- See if the license file exists.
            result.Status = LicenseStatus.MissingLicenseFile;
            //usePath = @"C:\inetpub\wwwroot\AccessClient\License\FinTrakIFRSLicense.lic";
            usePath = @"C:\Users\JARUMIEB\source\repos\SterlApplication\SterlApplication\Application\Presentation\Fintrak.Presentation.WebClient\License\FinTrakIFRSLicense.lic"; // Could be from Webconfig AppSetting
            if (string.IsNullOrEmpty(usePath))
                usePath = System.AppDomain.CurrentDomain.BaseDirectory + "/License/" + DefaultLicenseFile;
            if (File.Exists(usePath) == false)
                return result;

            // ----- Try to read in the file.
            result.Status = LicenseStatus.CorruptLicenseFile;
            try
            {
                licenseContent = new XmlDocument();
                licenseContent.Load(usePath);
            }
            catch (Exception ex)
            {
                // ----- Silent error.
                return result;
            }

            // ----- Prepare the public key resource for use.
            publicKey = RSA.Create();
            publicKey.FromXmlString(Properties.Resources.FinTrakPublicKey);

            // ----- Confirm the digital signature.
            try
            {
                signedDocument = new SignedXml(licenseContent);
                matchingNodes = licenseContent.GetElementsByTagName("Signature");
                signedDocument.LoadXml((XmlElement)matchingNodes[0]);
            }
            catch (Exception ex)
            {
                // ----- Still a corrupted document.
                return result;
            }

            if (signedDocument.CheckSignature(publicKey) == false)
            {
                result.Status = LicenseStatus.InvalidSignature;
                return result;
            }

            // ----- The license file is valid. Extract its members.
            try
            {
                // ----- Get the licensee name.
                matchingNodes = licenseContent.GetElementsByTagName("Licensee");
                result.Licensee = matchingNodes[0].InnerText;

                // ----- Get the license date.
                matchingNodes = licenseContent.GetElementsByTagName("LicenseDate");
                result.LicenseDate = Conversions.ToDate(matchingNodes[0].InnerText);

                // ----- Get the expiration date.
                matchingNodes = licenseContent.GetElementsByTagName("ExpireDate");
                result.ExpireDate = Conversions.ToDate(matchingNodes[0].InnerText);

                // ----- Get the version number.
                matchingNodes = licenseContent.GetElementsByTagName("CoveredVersion");
                result.CoveredVersion = matchingNodes[0].InnerText;

                // ----- Get the Product.
                matchingNodes = licenseContent.GetElementsByTagName("Product");
                result.Product = matchingNodes[0].InnerText;
            }
            catch (Exception ex)
            {
                // ----- Still a corrupted document.
                return result;
            }




            // ----- Check for out-of-range dates.

            if (InternetDateTime() != null && InternetDateTime() == DateAndTime.Today)
            {
                currentdate = InternetDateTime();
            }
            else
            {
                currentdate = DateAndTime.Today;
            }


            //string[] dateString = "7/1/2020".Split('/');
            //DateTime test = Convert.ToDateTime(dateString[1] + "/" + dateString[0] + "/" + dateString[2]);
            if (result.LicenseDate > currentdate)
            {
                result.Status = LicenseStatus.NotYetLicensed;
                return result;
            }

            if (result.ExpireDate < currentdate)
            {
                result.Status = LicenseStatus.LicenseExpired;
                return result;
            }

            // Check for Licensee
            if (result.Licensee != currentAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company)
            {
                result.Status = LicenseStatus.WrongLicensee;
                return result;
            }

            //Check for Product
            if (result.Product != currentAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product)
            {
                result.Status = LicenseStatus.WrongProductLicense;
                return result;
            }

            // ----- Check the version.
            versionParts = Strings.Split(result.CoveredVersion, ".");
            var loopTo = Information.UBound(versionParts);
            for (counter = 0; counter <= loopTo; counter++)
            {
                if (Information.IsNumeric(versionParts[counter]) == true)
                {
                    // ----- The version format is major.minor.build.revision.
                    switch (counter)
                    {
                        case 0:
                            {
                                comparePart = (currentAssembly.GetName().Version.Major).ToString();
                                break;
                            }

                        case 1:
                            {
                                comparePart = (currentAssembly.GetName().Version.Minor).ToString();
                                break;
                            }

                        case 2:
                            {
                                comparePart = (currentAssembly.GetName().Version.Build).ToString();
                                break;
                            }

                        case 3:
                            {
                                comparePart = (currentAssembly.GetName().Version.Revision).ToString();
                                break;
                            }

                        default:
                            {
                                // ----- Corrupt version number.
                                return result;
                            }
                    }

                    if (Conversion.Val(comparePart) != Conversion.Val(versionParts[counter]))
                    {
                        result.Status = LicenseStatus.VersionMismatch;
                        return result;
                    }
                }
            }

            // ----- Everything seems to be in order.
            result.Status = LicenseStatus.ValidLicense;       
            return result;
            
        }

        //public DateTime sendMailDate = DateTime.Today.AddDays(30);
        static string[] dateString = "5/12/2020".Split('/');
        public static DateTime sendMailDatetest = Convert.ToDateTime(dateString[1] + "/" + dateString[0] + "/" + dateString[2]);
        public DateTime sendMailDate = sendMailDatetest.AddDays(30);
        public void LicenseExpire()
        {
            string path = @"C:\Users\FIN\source\repos\license\saveddates.txt";
            //path = "saveddates.txt";
            string readText = File.ReadAllText(path);
            List<string> savedDates = JsonConvert.DeserializeObject<List<string>>(readText);
            string todayToString;
            if (ExamineLicense().ExpireDate < sendMailDate)
            {
                todayToString = DateTime.Today.ToString("MM/dd/yyyy");
                if (!savedDates.Contains(todayToString))
                {
                    savedDates.Add(todayToString);
                    File.WriteAllText(path, JsonConvert.SerializeObject(savedDates).ToString());
                    ///Send mail here
                    SMPTMail sMPTMail = new SMPTMail();
                    sMPTMail.button1_Click();
                }
            }
        }
    }
}
