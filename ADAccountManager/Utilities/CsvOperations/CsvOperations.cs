using ADAccountManager.Utilities.CsvService;
using ADAccountManager.Utilities.UserService;
using System.DirectoryServices.AccountManagement;

public class CsvOperations
{
    private readonly ICsvService _csvService;
    private readonly IUserPrincipalCreator _userPricipalCreator;

    internal CsvOperations(ICsvService csvService, IUserPrincipalCreator userPricipalCreator)
    {
        _csvService = csvService;
        _userPricipalCreator = userPricipalCreator;
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
                bool userCreated = await _userPricipalCreator.CreateUserAsync(user);

                if (!userCreated)
                    return false;
            }

            return true;
        }
        catch (PrincipalException e)
        {
            throw new ApplicationException("An error occurred while creating a new user.", e);
        }
    }
}