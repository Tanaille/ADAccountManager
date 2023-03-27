using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.GroupService
{
    internal static class GroupPrincipalExistenceCheck
    {
        internal static async Task<bool> Exists(string groupPrincipalName, PrincipalContext context)
        {
            try
            {
                IGroupPrincipalFinder groupPrincipalFinder = new GroupPrincipalFinder(context);
                using GroupPrincipal groupPrincipal = await groupPrincipalFinder.GetGroupPrincipalAsync(groupPrincipalName);

                if (groupPrincipal is not null)
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
                e.Data.Add("UserMessage", "More than one matching group principals were found. Contact your " +
                    "Active Directory administrator to review the existing groups and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while retrieving the group principal from Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
