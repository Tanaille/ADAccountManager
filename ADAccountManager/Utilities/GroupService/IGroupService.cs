using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal interface IGroupService
    {
        Task<bool> AddUserToGroupAsync(UserPrincipal user, string groupName);
    }
}
