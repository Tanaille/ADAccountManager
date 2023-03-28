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
        /// <summary>
        /// Checks whether a group principal exists within the directory. 
        /// </summary>
        /// <param name="groupPrincipalName">Name of the group principal to be checked.</param>
        /// <param name="context">The domain context to searched.</param>
        /// <returns>True if the group principal exists, false if it does not exist.</returns>
        internal static async Task<bool> Exists(string groupPrincipalName, PrincipalContext context)
        {
            try
            {
                IGroupPrincipalFinder groupPrincipalFinder = new GroupPrincipalFinder(context);
                using GroupPrincipal groupPrincipal = await groupPrincipalFinder.GetGroupPrincipalAsync(groupPrincipalName);

                return (groupPrincipal is not null);
            }
            catch (PrincipalServerDownException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                        "Check connectivity to the server.");
                
                throw;
            }
            catch (MultipleMatchesException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "More than one matching group principals were found. Contact your " +
                        "Active Directory administrator to review the existing groups and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "An error occurred while retrieving the group principal from Active Directory. " +
                        "See the log file for more information.");

                throw;
            }
        }
    }
}
