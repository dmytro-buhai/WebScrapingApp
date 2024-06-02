using SeleniumScraper.CommandsDomain.Abstract;
using System.Text;

namespace SeleniumScraper.Services.Commands
{
    public class CommandService(ICommandProcessor commandProcessor, IUserInterfaceService userInterfaceService)
    {
        public ICommandProcessor CommandProcessor { get; set; } = commandProcessor;

        public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

        private string _info = string.Empty;

        public void Start()
        {
            SetupInfo();

            while (true)
            {
                UserInterfaceService.DisplayMessage(_info);
                UserInterfaceService.DisplayMessage("Execute command №: ");
                var input = UserInterfaceService.ReadInput();

                if (string.IsNullOrWhiteSpace(input)
                    ||
                    !int.TryParse(input, out var command))
                {
                    continue;
                }

                foreach (var cmd in CommandProcessor.Commands)
                {
                    if (cmd.Id == command)
                    {
                        CommandProcessor.Process(command);
                    }
                }
                UserInterfaceService.DisplayMessage("RETURN to continue...");
                UserInterfaceService.ReadInput();
            }
        }

        private void SetupInfo()
        {
            var sb = new StringBuilder();
            var commands = CommandProcessor.Commands;

            sb.AppendLine("Enter command number:");

            foreach (var command in commands)
            {
                sb.AppendLine($"{command.Id}. {command.DisplayCommandName}");
            }
            _info = sb.ToString();
        }
    }
}