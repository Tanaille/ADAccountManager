using ADAccountManager.Models;

namespace ADAccountManager.Utilities.GroupService
{
    internal interface IGroupPrincipalCreator
    {
        Task<bool> CreateGroupPrincipalAsync(ADGroup group);
    }
}
