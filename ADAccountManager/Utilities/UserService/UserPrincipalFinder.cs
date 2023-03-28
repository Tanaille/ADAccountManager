using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal class UserPrincipalFinder : IUserPrincipalFinder
    {
        private readonly PrincipalContext _context;

        public UserPrincipalFinder(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve a user principal from the directory.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be retrieved.</param>
        /// <returns>The user principal object specified in the parameter.</returns>
        public async Task<UserPrincipal> GetUserPrincipalAsync(string userPrincipalName)
        {
            try
            {
                return await Task.Run(() => UserPrincipal.FindByIdentity(_context, userPrincipalName));
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (MultipleMatchesException e)
            {
                e.Data.Add("UserMessage", "More than one matching user principals were found. Contact your " +
                    "Active Directory administrator to review the existing users and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while retrieving the user principal from Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
