using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCBHarmonization.Models
{
    public class PersonModel
    {
        //public int Id { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Trusted_Connection { get; set; }
        public string MultipleActiveResultSets { get; set; }
        public string Integrated_Security { get; set; }
    }
}
