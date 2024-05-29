using BetterConsoleTables;
using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;
using System.Data;

namespace SeleniumScraper.CommandsDomain.Commands
{
    public class DisplayParsedComments(
        ProcessedData processedData, 
        IUserInterfaceService userInterfaceService, 
        ILogger<DisplayParsedComments> logger) : ICommand
    {
        private const int MaxColumnWidth = 30;

        public IUserInterfaceService UserInterfaceService => userInterfaceService;

        public string DisplayCommandName => "Display parsed comments";

        public int Id => KnownCommands.DisplayParsedComments;

        public void ExecuteCommand()
        {
            DisplayDataTable(processedData.Data);
        }

        void DisplayDataTable(DataTable table)
        {
            try
            {
                var columns = table.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();
                var myTable = new Table
                {
                    Config = TableConfiguration.Unicode()
                };

                myTable.AddColumns(Alignment.Left, Alignment.Left, columns);

                foreach (DataRow row in table.Rows)
                {
                    object[] truncatedRow = row.ItemArray.Select(item => TruncateString(item?.ToString(), MaxColumnWidth)).ToArray();
                    myTable.AddRow(truncatedRow);
                }

                UserInterfaceService.DisplayMessageInline(myTable.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception when displaying parsed comments: {ex.Message}");
                UserInterfaceService.DisplayMessage(string.Empty);
            }
        }

        static string TruncateString(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Length <= maxLength ? value : string.Concat(value.AsSpan(0, maxLength), "...");
        }
    }
}
