using ADAccountManager.Models;
using CsvHelper;
using System.Globalization;

namespace ADAccountManager.Utilities.CsvService
{
    internal class CsvService : ICsvService
    {
        public async Task<IEnumerable<User>> ReadUsersFromCsvAsync(string csvPath)
        {
            using StreamReader reader = new StreamReader(csvPath);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return await Task.Run(() => csv.GetRecords<User>().ToList());
        }
    }
}
