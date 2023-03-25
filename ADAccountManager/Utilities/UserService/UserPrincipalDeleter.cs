using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.UserService
{
    internal class UserPrincipalDeleter : IUserPrincipalDeleter
    {
        private readonly PrincipalContext _context;
        private readonly IUserPrincipalFinder _userFinder;

        public UserPrincipalDeleter(PrincipalContext context, IUserPrincipalFinder userFinder)
        {
            _context = context;
            _userFinder = userFinder;
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

                using var user = await _userFinder.GetUserAsync(userPrincipalName);
                
                if (user is null)
                    return false;

                await Task.Run(() => user.Delete());

                return true;
            }
            catch (PrincipalException e)
            {
                throw new ApplicationException("An error occurred while deleting a user.", e);
            }
        }
    }
}
