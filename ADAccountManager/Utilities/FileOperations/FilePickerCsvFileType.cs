using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.FileOperations
{
    class FilePickerCsvFileType
    {
        FilePickerFileType customFileType = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                            { DevicePlatform.WinUI, new[] { ".csv" } }, // CSV file extension
                        });
    }
}
