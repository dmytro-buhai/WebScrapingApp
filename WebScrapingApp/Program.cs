using NLog;
using SeleniumScraper;
using SeleniumScraper.Commands;

public class Program
{
    private static readonly ConsoleColor DefaultConsoleForegroundColor = Console.ForegroundColor;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static void Main(string[] args)
    {
        Logger.Info("Program starting...");

        var edgeLauncher = new EdgeLauncher(Logger);

        //var twitterURL = "https://twitter.com/home";

        var url = WriteMessageWaitForInputData("Enter URL to navigate: ");

        var navigateCommand = new NavigateCommand(edgeLauncher.GetEdgeDriver(), Logger, url);

        navigateCommand.Execute();

        Thread.Sleep(5000);

        edgeLauncher.Dispose();
    }

    public static string WriteMessageWaitForInputData(string message)
    {
        WriteMessage(message);
        return ReadInput();
    }

    private static void WriteMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(message);
    }

    private static string ReadInput()
    {
        var input = Console.ReadLine();

        while (string.IsNullOrEmpty(input))
        {
            Console.Write("Data is empty, try again: ");
            input = Console.ReadLine();
        }

        Console.ForegroundColor = DefaultConsoleForegroundColor;
        return input;
    }
}