//using Rebex.Net;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using SCBHarmonization.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Web;
using Tamir.SharpSsh;
//using WinSCP;

namespace SCBHarmonization.Helper
{
    public class RandomPassword
    {
        private readonly Random _random = new Random();
       
        
        public string RandomString(int size, bool lowerCase = false)
        {

            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public string RandomPasswordGenerator()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        //public string SfptDownload()
        //{
        //    string localFile = @"C:\upload";
        //    string host = "192.168.94.118";
        //    string username = "tester";
        //    string password = "password";
        //    var keyStr = new PrivateKeyFile(@"C:\Users\FINTRAK\Downloads\RebexTinySftpServer-Binaries-Latest\server-private-key-dss.ppk");
        //    string localFileName = System.IO.Path.GetFileName(localFile);
        //    string remoteFileName = @"C:\Users\FINTRAK\Downloads\RebexTinySftpServer-Binaries-Latest\data";
        //    string statusmsg = "Failed";
        //    try
        //    {
        //        using (var sftp = new SftpClient(host, username, password))
        //        {
        //            sftp.Connect();

        //            using (var file = File.OpenWrite(localFileName))
        //            {
        //                sftp.DownloadFile(remoteFileName, file);
        //            }

        //            sftp.Disconnect();
        //            statusmsg = "Successful";
        //            return statusmsg;
        //        }
        //    } catch(Exception ex)
        //    {
        //        statusmsg = ex.Message;
        //        throw ex.InnerException;
        //    }
        //}

        //public string SfptDownload()
        //{
        //    string statusmsg = "Failed";
        //    //try
        //    //{
        //    //    // Setup session options
        //    //    SessionOptions sessionOptions = new SessionOptions
        //    //    {
        //    //        SshPrivateKeyPath = "C:/Users/FINTRAK/Downloads/RebexTinySftpServer-Binaries-Latest/server-private-key-rsa.ppk",
        //    //        Protocol = Protocol.Sftp,
        //    //        HostName = "192.168.94.118",
        //    //        UserName = "tester",
        //    //        Password = "password"
        //    //        //SshHostKeyFingerprint = @"ssh-rsa 2048 aes256-cbc:AAAAB3NzaC1yc2EAAAADAQABAAABAQDEroHS+hqBLRSEluXNL9mkHafD9pscSS+n",
        //    //    };

        //    //    using (Session session = new Session())
        //    //    {
        //    //        // Connect
        //    //        session.Open(sessionOptions);

        //    //        // Upload files
        //    //        TransferOptions transferOptions = new TransferOptions();
        //    //        transferOptions.TransferMode = TransferMode.Binary;

        //    //        TransferOperationResult transferResult;
        //    //        transferResult =
        //    //            session.GetFiles("C:/Users/FINTRAK/Downloads/RebexTinySftpServer-Binaries-Latest/data/*", @"C:\upload\", false, transferOptions);

        //    //        // Throw on any error
        //    //        transferResult.Check();

        //    //        // Print results
        //    //        foreach (TransferEventArgs transfer in transferResult.Transfers)
        //    //        {
        //    //            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
        //    //        }
        //    //    }

        //    //    statusmsg = "Successful";
        //    //    return statusmsg;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    statusmsg = ex.Message;
        //    //    throw ex.InnerException;
        //    //}


        //    string hostname = "192.168.94.118";
        //    string username = "tester";
        //    string password = "password";
        //    try
        //    {
        //        using (var sftp = new Rebex.Net.Sftp())
        //        {
        //            // connect to a server
        //            sftp.Connect(hostname);

        //            // verify server's fingerprint
        //            // (see Security section for details)
        //            // ...

        //            // authenticate
        //            sftp.Login(username, password);

        //            // upload a file
        //            //sftp.Upload(@"C:\MyData\file1.txt", "/MyData");

        //            // download a file
        //            sftp.Download("C:/Users/FINTRAK/Downloads/RebexTinySftpServer-Binaries-Latest/data/testfile.txt", @"C:\upload");

        //            // disconnect (not required, but polite)
        //            sftp.Disconnect();
        //            statusmsg = "Successful";
        //        }
        //        return statusmsg;
        //    }
        //    catch(Exception ex)
        //    {
        //        statusmsg = ex.Message;
        //        throw ex.InnerException;
        //    }

        //}

        //public string SfptDownload()
        //{
        //    string statusmsg = "Failed";
        //    string remotepath = "C:/Users/FINTRAK/Downloads/RebexTinySftpServer-Binaries-Latest/data/testfile.txt";
        //    string localpath = HttpContext.Current.Server.MapPath("/upload");
        //    try
        //    {
        //        DownloadFile(remotepath, localpath);
        //        statusmsg = "Successful";
        //        return statusmsg;
        //    }
        //    catch (Exception ex)
        //    {
        //        statusmsg = ex.Message;
        //        throw ex.InnerException;
        //    }
        //}

        //void DownloadFile(string remotePath, string localPath)
        //{
        //    using (var client = GetClient()) {
        //        try
        //        {
        //            using (var s = new FileStream(localPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
        //            {
        //                client.BufferSize = 4 * 1024;
        //                client.DownloadFile(remotePath, s);
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            // Log error
        //            throw exception;
        //        }
        //        finally
        //        {
        //            client.Disconnect();
        //        }
        //    }
        //}

        //SftpClient GetClient()
        //{
        //    string url = "192.168.94.118";
        //    int port = 2222;
        //    string username = "tester";
        //    string password = "password";
        //    var connectionInfo = new PasswordConnectionInfo(url, port, username, password);

        //    var client = new SftpClient(connectionInfo);
        //    client.Connect();
        //    return client;
        //}

        //public bool DownloadSftpFiles(SFTPViewModel obj)
        //{
        //    bool result;
        //    obj.HostName = "ngwvapapp06a.zone2.scb.net";
        //    obj.UserName = "sshuser";
        //    obj.Port = 22;
        //    string _privateSshKeyPath = @"C:\Users\FINTRAK\Documents\Key\id_rsa.pem";
        //    Sftp sftp = new Sftp(obj.HostName, obj.UserName);
        //    sftp.AddIdentityFile(_privateSshKeyPath);
        //    sftp.Connect();
        //    ArrayList res = sftp.GetFileList(".");
        //    //upload
        //    sftp.Put(@"C:\Users\FINTRAK\Documents\SFTPFile", "/Datah");
        //    //download
        //    sftp.Get("*.xlsx", @"C:\Users\FINTRAK\Documents\SFTPFile");
        //    ArrayList Newres = sftp.GetFileList(".");
        //    result = true;
        //    sftp.Close();
        //    return result;
        //}

        public string SfptDownload()
        {
            string statusmsg = "Failed";
            string remotepath = @"\";
            //string remotepath = @"/Datah";
            string localpath = HttpContext.Current.Server.MapPath("/upload");
            try
            {
                DownloadFile(remotepath, localpath);
                statusmsg = "Successful";
                return statusmsg;
            }
            catch (Exception ex)
            {
                statusmsg = ex.Message;
                throw ex.InnerException;
            }
        }

        void DownloadFile(string remotePath, string localPath)
        {
            using (var client = GetClient())
            {
                try
                {
                    //using (var s = File.Create(localPath))
                    //{
                    //    client.BufferSize = 4 * 1024;
                    //    client.DownloadFile(remotePath, s);
                    //}

                    var files = client.ListDirectory(remotePath);

                    // Iterate over them
                    foreach (SftpFile file in files)
                    {

                        if (!file.IsDirectory && !file.IsSymbolicLink)
                        {
                            using (Stream fileStream = File.OpenWrite(Path.Combine(localPath, file.Name)))
                            {
                                client.DownloadFile(file.FullName, fileStream);
                                Debug.WriteLine(localPath);

                            }
                        }

                        else if (file.Name != "." && file.Name != "..")
                        {
                            Debug.WriteLine("Directory Ignored {0}", file.FullName);
                        }

                        else if (file.IsSymbolicLink)
                        {
                            Debug.WriteLine("Symbolic link ignored: {0}", file.FullName);
                        }


                    }

                }
                catch (Exception exception)
                {
                    // Log error
                    throw exception;
                }
                finally
                {
                    client.Disconnect();
                }
            }
        }

        SftpClient GetClient()
        {
            //string url = "192.168.234.96";
            string url = "10.205.21.18";
            int port = 2222;
            string username = "sshuser";
            string password = "password";
            //var connectionInfo = new PasswordConnectionInfo(url, port, username, password);

            var KeybasedMethod = new KeyboardInteractiveAuthenticationMethod(username);
            KeybasedMethod.AuthenticationPrompt +=
                (sender, e) => { e.Prompts.First().Response = password; };

            AuthenticationMethod[] methods = new AuthenticationMethod[]
            {
                //new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile(@"C:\Users\Fintrak\Downloads\RebexTinySftpServer-Binaries-Latest\id_rsa")),
                new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile(@"C:\IT\Key\DataH.ppk")),
                KeybasedMethod
            };
            ConnectionInfo connectionInfo = new ConnectionInfo(url, port, username, methods);

            var client = new SftpClient(connectionInfo);
            client.Connect();
            return client;
        }

        
    }
}