using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Models
{
    internal sealed class ADGroup
    {
        private readonly PrincipalContext _context;

        // Constructor
        public ADGroup(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a group principal from the directory.
        /// </summary>
        /// <param name="groupName">Name of the group principal to be retrieved.</param>
        /// <returns>The group principal named in the parameter. Returns null if the group is not found.</returns>
        public GroupPrincipal GetGroup(string groupPrinicpalName)
        {
            GroupPrincipal group = new GroupPrincipal(_context);
            try
            {
                // Check paramaters for nulls or empty strings
                ArgumentNullException.ThrowIfNullOrEmpty(groupPrinicpalName, nameof(groupPrinicpalName));

                // Find the group using the name provided by the groupPrincipalName parameter and returns it if found.
                group = GroupPrincipal.FindByIdentity(_context, groupPrinicpalName);
                return group;                
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return group;
            }
        }

        public bool Exists(string groupPrinicpalName)
        {
            try
            {
                // Check argument for a null value
                ArgumentNullException.ThrowIfNullOrEmpty(groupPrinicpalName);

                // Check whether a user principal exists. Return false if the user principal does exist
                using (var user = GetGroup(groupPrinicpalName))
                {
                    if (user == null)
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        public bool AddGroupMember(UserPrincipal user, string groupPrincipalName)
        {
            try
            {
                // Check arguments for null or empty values
                ArgumentNullException.ThrowIfNull(user);
                ArgumentNullException.ThrowIfNullOrEmpty(groupPrincipalName);

                using (GroupPrincipal group = GetGroup(groupPrincipalName))
                {
                    if (group == null)
                        return false;

                    group.Members.Add(_context, IdentityType.UserPrincipalName, user.UserPrincipalName);
                    group.Save();

                    return true;
                }
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }
    }
}
