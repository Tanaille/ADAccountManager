using ADAccountManager.Models;
using CsvHelper;
using System.Globalization;

namespace ADAccountManager.Utilities.CsvService
{
    internal class CsvService : ICsvService
    {
        /// <summary>
        /// Reads data from a CSV file and parses it into a collection of ADUser objects.
        /// </summary>
        /// <param name="csvPath">File path of the CSV file.</param>
        /// <returns>An IEnumerable of type ADUser.</returns>
        public async Task<IEnumerable<ADUser>> ReadUsersFromCsvAsync(string csvPath)
        {
            try
            {
                if (!File.Exists(csvPath))
                    throw new FileNotFoundException("The specified file was not found: " + csvPath);

                using StreamReader reader = new StreamReader(csvPath);
                using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                return await Task.Run(() => csv.GetRecords<ADUser>().ToList());
            }
            catch (IOException e)
            {
                e.Data.Add("UserMessage", "An error occurred while attempting to read from the file. " +
                    "See the log file for more information.");

                throw;
            }
        }
    }
}
