using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SeleniumScraper.CommandsDomain.Commands;

public class NavigateCommand(IUserInterfaceService userInterfaceService, 
    EdgeLauncher edgeLauncher, ILogger<NavigateCommand> logger) : ICommand
{
    public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

    private EdgeDriver EdgeDriver { get => edgeLauncher.GetEdgeDriver(); }

    //Facebook test URL: https://www.facebook.com/FoxNews/posts/846785273978003

    public string UrlToNavigate { get; set; } = "https://twitter.com/borisjohnson/status/1789204110417260575";

    public string DisplayCommandName => "Navigate to URL";

    public int Id { get => KnownCommands.NavigateCommand; }

    public void ExecuteCommand()
    {
        try
        {
            GetUrlFromUser();
            ValidateUrl(UrlToNavigate);

            logger.LogInformation($"Navigating to {UrlToNavigate}");

            EdgeDriver.Navigate().GoToUrl(UrlToNavigate);

            WebDriverWait wait = new WebDriverWait(EdgeDriver, TimeSpan.FromSeconds(5));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }
        catch (UriFormatException ex)
        {
            logger.LogError($"Invalid URL format: {ex.Message}");
        }
        catch (WebDriverException ex)
        {
            logger.LogError($"Webdriver error during navigation: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred: {ex.Message}");
        }
    }

    private void GetUrlFromUser()
    {
        var url = UserInterfaceService.ReadInput("Please provide a URL to navigate (or press Enter to use default): ");
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
