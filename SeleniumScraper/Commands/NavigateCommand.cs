using NLog;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper.Commands;

public class NavigateCommand(EdgeDriver webDriver, ILogger logger, string urlToNavigate) : EdgeCommand(webDriver, logger)
{
    public string UrlToNavigate { get; set; } = urlToNavigate;

    public override void Execute()
    {
        try
        {
            Logger.Info($"Navigate to {UrlToNavigate}");
            WebDriver.Url = UrlToNavigate;
        } 
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}
