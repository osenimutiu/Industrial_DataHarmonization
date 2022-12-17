using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class SFTPViewModel
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        //public string IPAddress { get; set; }
        public string SshHostKeyFingerprint { get; set; }
        public string PublicKeyPath { get; set; }
        public int Port { get; set; }
    }

    public class SFTPViemModelB
    {
        public string DownloadDate { get; set; }
    }
}