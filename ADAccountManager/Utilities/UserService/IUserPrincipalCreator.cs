using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserPrincipalCreator
    {
        Task<bool> CreateUserPrincipalAsync(ADUser user);
    }
}
