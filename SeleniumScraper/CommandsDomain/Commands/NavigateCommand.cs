using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using SeleniumScraper.CommandsDomain.Abstract;
using System.Text.RegularExpressions;

namespace SeleniumScraper.CommandsDomain.Commands;

public class NavigateCommand(EdgeLauncher edgeLauncher, ILogger logger) : ICommand
{
    private EdgeDriver EdgeDriver { get => edgeLauncher.GetEdgeDriver(); }

    public string UrlToNavigate { get; set; } = "https://twitter.com/nafoviking/status/1786415483924672635";

    public string DisplayCommandName => "Navigate to URL";

    public int Id { get => KnownCommands.NavigateCommand; }

    public void ExecuteCommand()
    {
        try
        {
            GetUrlFromUser();
            ValidateUrl(UrlToNavigate);

            logger.LogInformation($"Navigating to {UrlToNavigate}");
            EdgeDriver.Url = UrlToNavigate;
        }
        catch (UriFormatException ex)
        {
            logger.LogError("Invalid URL format.", ex);
        }
        catch (WebDriverException ex)
        {
            logger.LogError("Webdriver error during navigation.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError("An unexpected error occurred.", ex);
        }
    }

    private void GetUrlFromUser()
    {
        Console.Write("Please provide a URL to navigate (or press Enter to use default): ");
        string url = Console.ReadLine()!;
        UrlToNavigate = string.IsNullOrEmpty(url) ? UrlToNavigate : url;
    }

    private void ValidateUrl(string url)
    {
        var pattern = @"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        if (!regex.IsMatch(url))
        {
            throw new UriFormatException("Invalid URL format.");
        }
    }
}
