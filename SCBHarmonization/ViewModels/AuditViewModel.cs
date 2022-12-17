using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.ViewModels
{
    public class AuditViewModel
    {
        public int AUDITID { get; set; }
        public System.DateTime DATETIME { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DETAIL { get; set; }
        public string IPADDRESS { get; set; }
        public string URL { get; set; }
        public string DEVICENAME { get; set; }
        public string OSNAME { get; set; }
    }
    
}