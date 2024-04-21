using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper
{
    public class EdgeLauncher : IDisposable
    {
        private const string EdgeDriverPath = @"C:\Dev\drivers\msedgedriver.exe";
        private readonly string EdgeUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data1");
        private readonly ILogger<EdgeLauncher> _logger;
        
        public EdgeDriverService EdgeDriverService {get; private set;}
        public EdgeOptions EdgeOptions {get; private set;}

        private EdgeDriver? Driver { get; set; } 

        public EdgeLauncher(ILogger<EdgeLauncher> logger)
        {
            EdgeDriverService = EdgeDriverService.CreateDefaultService(EdgeDriverPath);
            EdgeDriverService.HideCommandPromptWindow = true;

            EdgeOptions = new EdgeOptions();
            EdgeOptions.AddArgument("--remote-debugging-port=9222");
            EdgeOptions.AddArgument($"--user-data-dir={EdgeUserDataPath}");
            EdgeOptions.AddArgument("--profile-directory=Profile 1");
            
            _logger = logger;
        }

        public EdgeDriver GetEdgeDriver() => Driver!;

        public void StartLauncher()
        {
            _logger.LogInformation("EdgeLauncher - Started");
            Driver = new EdgeDriver(EdgeDriverService, EdgeOptions);
        }

        public void StopLauncher()
        {
            Driver?.Quit();
        }

        public void SendKeys(string keys)
        { }

        public void Dispose()
        {
            Driver?.Quit();
            _logger.LogInformation($"EdgeLauncher - Stoped");
            GC.SuppressFinalize(this);
        }
    }
}
