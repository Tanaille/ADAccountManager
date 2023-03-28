using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Utilities.CsvOperations
{
    internal interface ICsvOperations
    {
        Task<List<ADUser>> CreateUserPrincipalsFromCsvAsync(string csvPath);
    }
}
