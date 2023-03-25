using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal class UserFinder : IUserFinder
    {
        private readonly PrincipalContext _context;

        public UserFinder(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a user principal from the directory.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be retrieved.</param>
        /// <returns>The user principal specified in the parameter.</returns>
        public async Task<UserPrincipal> GetUserAsync(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName, nameof(userPrincipalName));

                UserPrincipal user = await Task.Run(() => UserPrincipal.FindByIdentity(_context, userPrincipalName));

                return user;
            }
            catch (PrincipalOperationException e)
            {
                throw new ApplicationException("An error occurred while retrieving the user principal.", e);
            }
        }
    }
}
