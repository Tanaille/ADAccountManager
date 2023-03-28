using ADAccountManager.Models;
using ADAccountManager.Utilities.CsvOperations;
using ADAccountManager.Utilities.CsvService;
using ADAccountManager.Utilities.GroupService;
using ADAccountManager.Utilities.UserService;
using System.DirectoryServices.AccountManagement;

public class CsvOperations : ICsvOperations
{
    private readonly ICsvService _csvService;
    private readonly IUserPrincipalCreator _userPricipalCreator;
    private readonly PrincipalContext _context;

    internal CsvOperations(ICsvService csvService, IUserPrincipalCreator userPricipalCreator, PrincipalContext context)
    {
        _csvService = csvService;
        _userPricipalCreator = userPricipalCreator;
        _context = context;
    }

    /// <summary>
    /// Creates one or more user principals from a CSV file.
    /// </summary>
    /// <param name="csvPath">File path of the CSV file.</param>
    /// <returns>A list of type ADUser containing the user principals that were not successfully created.</returns>
    public async Task<List<ADUser>> CreateUserPrincipalsFromCsvAsync(string csvPath)
    {
        try
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException("File not found: " + csvPath);

            List<ADUser> notCreatedUserPrincipals = new List<ADUser>();

            var userPrincipals = await _csvService.ReadUsersFromCsvAsync(csvPath);

            foreach (var userPrincipal in userPrincipals)
            {
                bool userCreated = false;

                if (!await UserPrincipalExistenceCheck.Exists(userPrincipal.UserPrincipalName, _context))
                    userCreated = await _userPricipalCreator.CreateUserPrincipalAsync(userPrincipal);

                if (!userCreated)
                    notCreatedUserPrincipals.Add(userPrincipal);
            }

            return notCreatedUserPrincipals;
        }
        catch (PrincipalOperationException e)
        {
            if (!e.Data.Contains("UserMessage"))
                e.Data.Add("UserMessage", "An error occurred while updating the directory store (CREATE operation failed). " +
                    "See the log file for more information.");

            throw;
        }
        catch (PrincipalExistsException e)
        {
            if (!e.Data.Contains("UserMessage"))
                e.Data.Add("UserMessage", "The user principal already exists in the directory.");

            throw;
        }
        catch (PrincipalServerDownException e)
        {
            if (!e.Data.Contains("UserMessage"))
                e.Data.Add("UserMessage", "The Active Directory server could not be reached. " +
                    "Check connectivity to the server.");

            throw;
        }
        catch (Exception e)
        {
            if (!e.Data.Contains("UserMessage"))
                e.Data.Add("UserMessage", "An error occurred while adding the user principal to Active Directory. " +
                    "See the log file for more information.");

            throw;
        }
    }
}