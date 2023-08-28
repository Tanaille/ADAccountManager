using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.Services
{
    public interface IActiveDirectoryService
    {
        PrincipalContext GetPrincipalContext();
    }
}
