using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;

namespace SeleniumScraper.CommandsDomain.Commands;

public class StartLauncherCommand(IUserInterfaceService userInterfaceService, 
    EdgeLauncher edgeLauncher, ILogger<StartLauncherCommand> logger) : ICommand
{
    public string DisplayCommandName => "Start driver";

    public int Id => KnownCommands.StartLauncherCommand;

    public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

    public void ExecuteCommand()
    {
        try
        {
            logger.LogInformation($"Starting web driver...");
            edgeLauncher.StartLauncher();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
