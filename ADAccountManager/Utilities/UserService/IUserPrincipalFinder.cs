using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserPrincipalFinder
    {
        Task<UserPrincipal> GetUserAsync(string userPrincipalName);
    }
}
