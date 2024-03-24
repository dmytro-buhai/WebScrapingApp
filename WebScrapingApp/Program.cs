// See https://aka.ms/new-console-template for more information
using NLog;
using SeleniumScraper;
using SeleniumScraper.Commands;

Logger logger = LogManager.GetCurrentClassLogger();
var defaultForegroundColor = Console.ForegroundColor;


logger.Info("Program starting...");

var edgeLauncher = new EdgeLauncher(logger);

var twitterURL = "https://twitter.com/home";

var url = WriteMessageWaitForInputData("Enter URL to navigate: ");

var navigateCommand = new NavigateCommand(edgeLauncher.GetEdgeDriver(), logger, url);

navigateCommand.Execute();

Thread.Sleep(5000);

edgeLauncher.Dispose();

string WriteMessageWaitForInputData(string message)
{
    WriteMessage(message);
    return ReadInput();
}

void WriteMessage(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(message);
}

string ReadInput()
{
    var input = Console.ReadLine();

    while (string.IsNullOrEmpty(input))
    {
        Console.Write("Data is empty, try again: ");
        input = Console.ReadLine();
    }

    Console.ForegroundColor = defaultForegroundColor;
    return input;
}