using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Models
{
    internal class Config
    {
        public string DomainName { get; set; }
        public string DomainUser { get; set; }
        public string DomainPassword { get; set; }
        public string DefaultDomainOU { get; set; }
    }
}
