using NLog;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper.Commands;

public abstract class EdgeCommand(EdgeDriver webDriver, ILogger logger) : ICommand
{
    protected readonly EdgeDriver WebDriver = webDriver;
    protected readonly ILogger Logger = logger;

    public abstract void Execute();
}
