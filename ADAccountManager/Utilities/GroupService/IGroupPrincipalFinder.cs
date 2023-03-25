using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal interface IGroupPrincipalFinder
    {
        Task<GroupPrincipal> GetGroupPrincipalAsync(string groupPrincipalName);
    }
}
