using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserPrincipalDeleter
    {
        Task<bool> DeleteUserPrincipalAsync(string userPrincipalName);
    }
}
