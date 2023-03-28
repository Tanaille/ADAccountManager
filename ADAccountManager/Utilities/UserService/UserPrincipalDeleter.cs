using System.DirectoryServices.AccountManagement;

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
        /// Delete an existing user principal from the directory.
        /// </summary>
        /// <param name="userPrincipalName">User principal name (such as name.surname) of the user to be deleted.</param>
        /// <returns>True if the deletion is successful. False if the deletion is unsuccessful (user principal does not exist).</returns>
        public async Task<bool> DeleteUserPrincipalAsync(string userPrincipalName)
        {
            try
            {
                using var user = await _userFinder.GetUserPrincipalAsync(userPrincipalName);
                
                if (user is null)
                    return false;

                await Task.Run(() => user.Delete());

                return true;
            }
            catch (PrincipalOperationException e)
            {
                e.Data.Add("UserMessage", "An error occurred while updating the directory store (DELETE operation failed). " +
                    "See the log file for more information.");

                throw;
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
                    "Active Directory administrator to review the existing groups and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while adding the user principal to Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
