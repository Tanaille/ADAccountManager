using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.UserService
{
    internal class UserPrincipalCreator : IUserPrincipalCreator
    {
        private readonly PrincipalContext _context;

        public UserPrincipalCreator(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new user principal object in the directory.
        /// </summary>
        /// <param name="user">User principal object containing the user account information.</param>
        /// <returns>True if the user principal creation is successful. False if the user account
        /// creation is unsuccessful (ADUser object is null).</returns>
        public async Task<bool> CreateUserPrincipalAsync(ADUser user)
        {
            try
            {
                if (user is null)
                    return false;

                //using UserPrincipal userPrincipal = new UserPrincipal(_context)
                using InetOrgPerson userPrincipal = new InetOrgPerson(_context)
                {
                    Name = user.UserPrincipalName,
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    UserPrincipalName = user.UserPrincipalName + "@" + user.Domain,
                    SamAccountName = user.UserPrincipalName,
                    DisplayName = user.FirstName + " " + user.LastName,
                    Description = user.FirstName + " " + user.LastName,
                    MobilePhone = user.MobilePhone,
                    EmailAddress = user.UserPrincipalName + "@" + user.Domain,
                    ProxyAddresses = new string[] { "SMTP:" + user.UserPrincipalName + "@" + user.Domain},
                    UsageLocation = "ZA",
                    Enabled = true
                };

                await Task.Run(() => userPrincipal.Save());

                return true;
            }
            catch (PrincipalOperationException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "An error occurred while updating the directory store (CREATE operation failed). " +
                        "See the log file for more information.");

                throw;
            }
            catch (PrincipalServerDownException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                        "Check connectivity to the server.");

                throw;
            }
            catch (PrincipalExistsException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "The user principal already exists in the directory.");

                throw;
            }
            catch (Exception e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "An error occurred while adding the user principal to Active Directory. " +
                        "See the log file for more information.");

                throw;
            }
        }
    }
}
