using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserPrincipalCreator
    {
        Task<bool> CreateUserAsync(ADUser user);
    }
}
