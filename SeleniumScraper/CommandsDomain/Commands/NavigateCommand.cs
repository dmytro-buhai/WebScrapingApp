using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Edge;
using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain.Commands;

public class NavigateCommand(EdgeDriver webDriver, ILogger logger) : EdgeCommand(webDriver, logger)
{
    public string UrlToNavigate { get; set; }

    public override string DisplayCommandName => "Navigate to URL";

    public override int Id { get => KnownCommands.NavigateCommand; }

    public override void ExecuteCommand()
    {
        try
        {
            Console.Write("Please provide an url to navigate: ");
            UrlToNavigate = Console.ReadLine();
            Logger.LogInformation($"Navigate to {UrlToNavigate}");
            WebDriver.Url = UrlToNavigate;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
        }
    }
}
