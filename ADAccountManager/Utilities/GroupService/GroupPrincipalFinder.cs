using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupPrincipalFinder : IGroupPrincipalFinder
    {
        private readonly PrincipalContext _context;

        public GroupPrincipalFinder(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a group principal from the directory.
        /// </summary>
        /// <param name="groupPrincipalName">Principal name (such as groupe.name) of the group to be retrieved.</param>
        /// <returns>The group principal object specified in the parameter.</returns>
        public async Task<GroupPrincipal> GetGroupPrincipalAsync(string groupPrincipalName)
        {
            try
            {
                GroupPrincipal groupPrincipal = await Task.Run(() => GroupPrincipal.FindByIdentity(_context, groupPrincipalName));

                return groupPrincipal;
            }
            catch (PrincipalServerDownException e)
            {
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

                throw;
            }
            catch (MultipleMatchesException e)
            {
                e.Data.Add("UserMessage", "More than one matching group principals were found. Contact your " +
                    "Active Directory administrator to review the existing groups and remvoe duplicates.");

                throw;
            }
            catch (Exception e)
            {
                e.Data.Add("UserMessage", "An error occurred while retrieving the group principal from Active Directory.");

                throw;
            }
        }
    }
}
