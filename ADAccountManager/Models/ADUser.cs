using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Models
{
    public class ADUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Domain { get; set; }
    }
}
