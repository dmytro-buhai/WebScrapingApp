using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper.CommandsDomain.Abstract;

public abstract class EdgeCommand(EdgeDriver webDriver, ILogger logger) : ICommand
{
    protected readonly EdgeDriver WebDriver = webDriver;
    protected readonly ILogger Logger = logger;

    public abstract string DisplayCommandName { get; }

    public abstract int Id { get; }

    public abstract void ExecuteCommand();
}
