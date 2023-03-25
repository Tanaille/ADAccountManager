using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserDeleter
    {
        Task<bool> DeleteUserAsync(string userPrincipalName);
    }
}
