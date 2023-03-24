using ADAccountManager.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.UserService
{
    public class UserService : IUserService
    {
        private readonly PrincipalContext _context;

        // Constructor
        public UserService(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="firstName">Given name of the user.</param>
        /// <param name="lastName">Surname of the user.</param>
        /// <param name="userPrincipalName">User principal name, in the format "name.surname".</param>
        /// <param name="domain">Domain the user should be added to.</param>
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

        /// <summary>
        /// Get a user principal from the directory.
        /// </summary>
        /// <param name="username">Name of the user principal to be retrieved.</param>
        /// <returns>The user principal specified in the parameter.</returns>
        public async Task<UserPrincipal> GetUserAsync(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName, nameof(userPrincipalName));

                using UserPrincipal user = await Task.Run(() => UserPrincipal.FindByIdentity(_context, userPrincipalName));
                
                return user;
            }
            catch (PrincipalOperationException e)
            {
                throw new ApplicationException("An error occurred while retrieving the user principal.", e);
            }
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be deleted.</param>
        /// <returns>True if the deletion is successful. False if the deletion is unsuccessful.</returns>
        public async Task<bool> DeleteUserAsync(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName);

                using (var user = GetUserAsync(userPrincipalName))
                {
                    if (user.Result is null)
                        return false;

                    await Task.Run(() => user.Result.Delete());
                }

                return true;
            }
            catch (PrincipalException e)
            {
                throw new ApplicationException("An error occurred while deleting a user.", e);
            }
        }
    }
}
