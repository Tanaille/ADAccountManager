using ADAccountManager.Models;

namespace ADAccountManager.Utilities.UserService
{
    internal interface IUserCreator
    {
        Task<bool> CreateUserAsync(User user);
    }
}
