using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupPrincipalMemberAdder : IGroupPrincipalMemberAdder
    {
        /// <summary>
        /// Adds an Active Directory user principal to a group principal's member list.
        /// </summary>
        /// <param name="groupPrincipal">The group principal object to which the user principal will be added.</param>
        /// <param name="userPrincipal">The user principal object which is to be added to the group principal.</param>
        /// <returns>True if the user principal was successfully added to the group principal, 
        /// false the member add operation failed (if either the user principal object or group principal object is null).</returns>
        public async Task<bool> AddUserToGroupAsync(GroupPrincipal groupPrincipal, UserPrincipal userPrincipal)
        {
            try
            {
                if (groupPrincipal is null || userPrincipal is null)
                    return false;

                groupPrincipal.Members.Add(userPrincipal);
                await Task.Run(() => groupPrincipal.Save());

                return true;
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (PrincipalOperationException e)
            {
                e.Data.Add("UserMessage", "An error occurred while updating the directory store (MEMBER_ADD operation failed). " +
                    "See the log file for more information.");

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
                e.Data.Add("UserMessage", "An error occurred while adding the group principal to Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
