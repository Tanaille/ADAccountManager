using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.CsvOperations
{
    internal interface ICsvOperations
    {
        Task<CsvUserPrincipalCreationResult> CreateUserPrincipalsFromCsvAsync(string csvPath);
    }
}
