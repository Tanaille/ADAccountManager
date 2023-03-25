namespace ADAccountManager.Utilities.GroupService
{
    internal interface IGroupPrincipalDeleter
    {
        Task<bool> DeleteGroupPrincipalAsync(string groupPrincipalName);
    }
}
