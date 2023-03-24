using ADAccountManager.Utilities.CsvService;
using ADAccountManager.Utilities.GroupService;
using ADAccountManager.Utilities.UserService;
using System.DirectoryServices.AccountManagement;

public class CsvOperations
{
    private readonly ICsvService _csvService;
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;

    internal CsvOperations(ICsvService csvService, IUserService userService, IGroupService groupService)
    {
        _csvService = csvService;
        _userService = userService;
        _groupService = groupService;
    }

    public async Task<bool> CreateUsersFromCsvAsync(string csvPath, PrincipalContext context)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(csvPath);

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("File not found: " + csvPath);

            var users = await _csvService.ReadUsersFromCsvAsync(csvPath);

            foreach (var user in users)
            {
                bool userCreated = await _userService.CreateUserAsync(user);

                if (userCreated)
                {
                    using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, user.UserPrincipalName))
                    {
                        await _groupService.AddUserToGroupAsync(userPrincipal, "StaffAccounts");
                    }
                }
            }

            return true;
        }
        catch (PrincipalException e)
        {
            throw new ApplicationException("An error occurred while creating a new user.", e);
        }
    }
}