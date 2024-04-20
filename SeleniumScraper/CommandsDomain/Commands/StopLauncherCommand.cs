using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain.Commands;

public class StopLauncherCommand(EdgeLauncher edgeLauncher, ILogger logger) : ICommand
{
    public string DisplayCommandName => "Stop driver";

    public int Id { get => KnownCommands.StopLauncherCommand; }

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
