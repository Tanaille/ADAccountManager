using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly PrincipalContext _context;

        public ActiveDirectoryService()
        {
            // Customize the constructor based on your needs
            _context = new PrincipalContext(ContextType.Domain, "ad.ferrum.org.za", "OU=Dev,OU=UserAccounts,DC=ad,DC=ferrum,DC=org,DC=za", "fhs\\netadmin", "JRQT~vhul");
        }

        public PrincipalContext GetPrincipalContext()
        {
            return _context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
