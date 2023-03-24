using ADAccountManager.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.GroupService
{
    internal class GroupService : IGroupService
    {
        public async Task<bool> AddUserToGroupAsync(UserPrincipal user, string groupName)
        {
            try
            {
                ADGroup group = new ADGroup(new PrincipalContext(ContextType.Domain, "ferrum.local"));
                await Task.Run(() => group.AddGroupMember(user, groupName));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
