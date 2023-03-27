using ADAccountManager.Utilities.UserService;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.GroupService
{
    internal static class UserPrincipalExistenceCheck
    {
        internal static async Task<bool> Exists(string userPrincipalName, PrincipalContext context)
        {
            try
            {
                IUserPrincipalFinder userPrincipalFinder = new UserPrincipalFinder(context);
                using UserPrincipal userPrincipal = await userPrincipalFinder.GetUserPrincipalAsync(userPrincipalName);

                if (userPrincipal is not null)
                    return true;

                return false;
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (MultipleMatchesException e)
            {
                e.Data.Add("UserMessage", "More than one matching user principals were found. Contact your " +
                    "Active Directory administrator to review the existing users and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while retrieving the user principal from Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
