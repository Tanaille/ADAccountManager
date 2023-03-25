using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserPrincipalDeleter
    {
        Task<bool> DeleteUserAsync(string userPrincipalName);
    }
}
