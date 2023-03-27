using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupPrincipalDeleter : IGroupPrincipalDeleter
    {
        private readonly PrincipalContext _context;
        private readonly IGroupPrincipalFinder _groupPrincipalFinder;

        public GroupPrincipalDeleter(PrincipalContext context, IGroupPrincipalFinder groupPrincipalFinder)
        {
            _context = context;
            _groupPrincipalFinder = groupPrincipalFinder;
        }

        /// <summary>
        /// Delete an existing group.
        /// </summary>
        /// <param name="groupPrincipalName">Group principal name (such as group.name) of the group to be deleted.</param>
        /// <returns>True if the deletion is successful. False if the deletion is unsuccessful.</returns>
        public async Task<bool> DeleteGroupPrincipalAsync(string groupPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(groupPrincipalName);

                using var groupPrincipal = await _groupPrincipalFinder.GetGroupPrincipalAsync(groupPrincipalName);

                if (groupPrincipal is null)
                    return false;

                await Task.Run(() => groupPrincipal.Delete());

                return true;
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (PrincipalOperationException e)
            {
                e.Data.Add("UserMessage", "An error occurred while updating the directory store (DELETE operation failed). " +
                    "See the log file for more information.");

                throw;
            }
            catch (MultipleMatchesException e)
            {
                e.Data.Add("UserMessage", "More than one matching group principals were found. Contact your " +
                    "Active Directory administrator to review the existing groups and remove duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while retrieving the group principal from Active Directory. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
