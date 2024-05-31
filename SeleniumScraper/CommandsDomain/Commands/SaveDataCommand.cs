using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;
using System.Data;
using System.Text;

namespace SeleniumScraper.CommandsDomain.Commands
{
    public class SaveDataCommand(
        ProcessedData processedData,
        IUserInterfaceService userInterfaceService,
        ILogger<DisplayParsedComments> logger) : ICommand
    {
        private const string DataFilePath = @"C:\dev\temp";

        public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

        public string DisplayCommandName => "Save parsed comments";

        public int Id => KnownCommands.SaveParsedComments;

        public void ExecuteCommand()
        {
            logger.LogInformation("Saving data has started...");
            var result = SaveDataToFile(processedData.Data);
            if (result)
            {
                logger.LogInformation("Saving was finished sucessully.");
            }
            else
            {
                logger.LogError("Saving was failed.");
            }
        }

        public bool SaveDataToFile(DataTable table)
        {
            try
            {
                // Create the CSV content
                var csvContent = new StringBuilder();

                // Add the header rowW
                var headerRaw = string.Join(",", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                csvContent.AppendLine(headerRaw);

                // Add the data rows
                foreach (DataRow row in table.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        csvContent.Append("\"" + item.ToString().Replace("\n", " ") + "\",");
                    }
                    csvContent.AppendLine();
                }

                // Define the path to save the CSV file
                var filePath = Path.Combine(DataFilePath, $"{Guid.NewGuid()}.csv");

                // Write the CSV content to a file
                File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);

                logger.LogInformation($"CSV file saved to {filePath}");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
