using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupPrincipalMemberAdder : IGroupPrincipalMemberAdder
    {
        public async Task<bool> AddUserToGroupAsync(GroupPrincipal groupPrincipal, UserPrincipal userPrincipal)
        {
            try
            {
                if (userPrincipal is null)
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
        }
    }
}
