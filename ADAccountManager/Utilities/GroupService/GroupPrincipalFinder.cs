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
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(groupPrincipalName, nameof(groupPrincipalName));

                GroupPrincipal groupPrincipal = await Task.Run(() => GroupPrincipal.FindByIdentity(_context, groupPrincipalName));

                return groupPrincipal;
            }
            catch (PrincipalOperationException e)
            {
                throw new ApplicationException("An error occurred while retrieving the user principal.", e);
            }
        }
    }
}
