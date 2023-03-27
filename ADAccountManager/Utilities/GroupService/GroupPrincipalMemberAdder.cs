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
            catch (PrincipalOperationException e)
            {
                throw new ApplicationException("An error occurred while adding a user to a group.", e);
            }
        }
    }
}
