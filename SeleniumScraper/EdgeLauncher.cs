using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper
{
    public class EdgeLauncher : IDisposable
    {
        private const string EdgeDriverPath = @"C:\Dev\drivers\msedgedriver.exe";
        private readonly string EdgeUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data1");
        private readonly EdgeDriver _driver;
        private readonly ILogger _logger;

        public EdgeLauncher(ILogger logger)
        {
            var edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument($"--user-data-dir={EdgeUserDataPath}");
            edgeOptions.AddArgument("--profile-directory=Profile 1");

            _driver = new EdgeDriver(EdgeDriverPath, edgeOptions);
            _logger = logger;
        }

        public EdgeDriver GetEdgeDriver() => _driver;

        public void SendKeys(string keys) 
        { }

        public void StopLauncher()
        {
            _driver?.Quit();
        }

        public void Dispose()
        {
            _driver?.Quit();
            _logger.Log(NLog.LogLevel.Info, $"EdgeLauncher - Stoped");
            GC.SuppressFinalize(this);
        }
    }
}
