using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal class UserCreator : IUserCreator
    {
        private readonly PrincipalContext _context;

        public UserCreator(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="user">User object containing the user account information.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                using UserPrincipal userPrincipal = new UserPrincipal(_context)
                {
                    Name = user.UserPrincipalName,
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    UserPrincipalName = user.UserPrincipalName + "@" + user.Domain,
                    SamAccountName = user.UserPrincipalName,
                    DisplayName = user.FirstName + " " + user.LastName,
                    Description = user.FirstName + " " + user.LastName,
                    Enabled = true
                };

                await Task.Run(() => userPrincipal.Save());

                return true;
            }
            catch (PrincipalOperationException e)
            {
                throw new ApplicationException("An error occurred while creating a new user.", e);
            }
        }
    }
}
