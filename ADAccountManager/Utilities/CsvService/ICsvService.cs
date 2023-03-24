using ADAccountManager.Models;

namespace ADAccountManager.Utilities.CsvService
{
    internal interface ICsvService
    {
        Task<IEnumerable<User>> ReadUsersFromCsvAsync(string csvPath);
    }
}
