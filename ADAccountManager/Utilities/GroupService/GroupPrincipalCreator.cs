using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupPrincipalCreator : IGroupPrincipalCreator
    {
        private readonly PrincipalContext _context;

        public GroupPrincipalCreator(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Active Directory group principal.
        /// </summary>
        /// <param name="group">The ADGroup object that will be used to create the group.</param>
        /// <returns>True if group creation is successful, false if group creation is unsuccessful (group already exists, 
        /// or ADGroup object is null).</returns>
        public async Task<bool> CreateGroupPrincipalAsync(ADGroup group)
        {
            try
            {
                if (group is null || await GroupPrincipalExistenceCheck.Exists(group.Name, _context))
                    return false;

                using GroupPrincipal groupPrincipal = new GroupPrincipal(_context)
                {
                    Name = group.Name,
                    DisplayName = group.Name,
                    SamAccountName = group.Name,
                    IsSecurityGroup = true
                };

                await Task.Run(() => groupPrincipal.Save());

                return true;
            }
            catch (PrincipalOperationException e)
            {
                e.Data.Add("UserMessage", "An error occurred while updating the directory store (CREATE operation failed). " +
                    "See the log file for more information.");

                throw;
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (PrincipalExistsException e)
            {
                e.Data.Add("UserMessage", "The group principal already exists in the directory.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while adding the group principal to Active Directory. " +
                    "See the log file for more information.");
                
                throw;
            }
        }
    }
}
