using SeleniumScraper.CommandsDomain.Abstract;
using System.Text;

namespace SeleniumScraper.Services.Commands
{
    public class CommandService(ICommandProcessor commandProcessor)
    {
        private readonly ICommandProcessor _processor = commandProcessor;
        private string _info = string.Empty;

        public void Start()
        {
            SetupInfo();

            while (true)
            {
                Console.WriteLine(_info);
                Console.Write("Execute command №: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)
                    ||
                    !int.TryParse(input, out var command))
                {
                    continue;
                }

                foreach (var cmd in _processor.Commands)
                {
                    if (cmd.Id == command)
                    {
                        _processor.Process(command);
                    }
                }
                Console.WriteLine("RETURN to continue...");
                Console.ReadLine();
            }
        }

        private void SetupInfo()
        {
            var sb = new StringBuilder();
            var commands = _processor.Commands;

            sb.AppendLine("Enter command number:");

            foreach (var command in commands)
            {
                sb.AppendLine($"{command.Id}. {command.DisplayCommandName}");
            }
            _info = sb.ToString();
        }
    }
}