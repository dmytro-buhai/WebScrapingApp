using NLog;
using WebScrapingApp;

public class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static void Main(string[] args)
    {
        Logger.Info("Program starting...");

        Startup.StartApplication();

        //var twitterURL = "https://twitter.com/home";
    }
}