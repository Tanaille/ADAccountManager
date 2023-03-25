using ADAccountManager.Utilities.UserService;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch (PrincipalException e)
            {
                throw new ApplicationException("An error occurred while deleting a user.", e);
            }
        }
    }
}
