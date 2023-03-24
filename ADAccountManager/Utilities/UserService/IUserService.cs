using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(User user, PrincipalContext context);
    }
}
