using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserFinder
    {
        Task<UserPrincipal> GetUserAsync(string userPrincipalName);
    }
}
