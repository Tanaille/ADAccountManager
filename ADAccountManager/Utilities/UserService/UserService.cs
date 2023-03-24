using ADAccountManager.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.UserService
{
    public class UserService : IUserService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public async Task<bool> CreateUserAsync(User user, PrincipalContext context)
        {
            try
            {
                using UserPrincipal userPrincipal = new UserPrincipal(context)
                {
                    Name = user.UserPrincipalName,
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    UserPrincipalName = user.UserPrincipalName + "@" + user.Domain,
                    SamAccountName = user.UserPrincipalName,
                    DisplayName = user.FirstName + " " + user.LastName,
                    Description = user.FirstName + " " + user.LastName,
                    Enabled = true
                };

                await Task.Run(() => userPrincipal.Save());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
