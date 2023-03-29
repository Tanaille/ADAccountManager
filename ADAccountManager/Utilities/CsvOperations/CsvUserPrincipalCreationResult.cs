using ADAccountManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.CsvOperations
{
    public class CsvUserPrincipalCreationResult
    {
        public List<ADUser> CreatedUserPrincipals { get; set; }
        public List<ADUser> NotCreatedUserPrincipals { get; set; }

        public CsvUserPrincipalCreationResult(List<ADUser> createdUserPrincipals, List<ADUser> notCreatedUserPrincipals) 
        { 
            CreatedUserPrincipals = createdUserPrincipals;
            NotCreatedUserPrincipals = notCreatedUserPrincipals;
        }
    }
}
