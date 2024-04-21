using Microsoft.Extensions.Logging;
using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain.Commands;

public class StartLauncherCommand(EdgeLauncher edgeLauncher, ILogger logger) : ICommand
{
    public string DisplayCommandName => "Start driver";

    public int Id { get => KnownCommands.StartLauncherCommand; }

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
