using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal interface IGroupPrincipalMemberAdder
    {
        Task<bool> AddUserToGroupAsync(GroupPrincipal groupPrincipal, UserPrincipal userPrincipal);
    }
}
