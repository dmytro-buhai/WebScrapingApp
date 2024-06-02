using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;

namespace SeleniumScraper.CommandsDomain.Commands;

public class StopLauncherCommand(IUserInterfaceService userInterfaceService, 
    EdgeLauncher edgeLauncher, ILogger<StopLauncherCommand> logger) : ICommand
{
    public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

    public string DisplayCommandName => "Stop driver";

    public int Id => KnownCommands.StopLauncherCommand;

    public void ExecuteCommand()
    {
        try
        {
            edgeLauncher.StopLauncher();
            logger.LogInformation($"Stoping web driver...");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
