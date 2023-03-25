using ADAccountManager.Models;

namespace ADAccountManager.Utilities.CsvService
{
    internal interface ICsvService
    {
        Task<IEnumerable<ADUser>> ReadUsersFromCsvAsync(string csvPath);
    }
}
