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

        public async Task<bool> CreateGroupPrincipalAsync(ADGroup group)
        {
            try
            {
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
                throw new ApplicationException("An error occurred while creating a new group.", e);
            }
        }
    }
}
