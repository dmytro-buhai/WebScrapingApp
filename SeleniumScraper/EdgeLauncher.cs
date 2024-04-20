using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Edge;

namespace SeleniumScraper
{
    public class EdgeLauncher : IDisposable
    {
        private const string EdgeDriverPath = @"C:\Dev\drivers\msedgedriver.exe";
        private readonly string EdgeUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data1");
        private readonly ILogger<EdgeLauncher> _logger;
        
        private readonly EdgeDriverService _edgeDriverService;
        private readonly EdgeOptions _edgeOptions;

        private EdgeDriver? Driver { get; set; } 

        public EdgeLauncher(ILogger<EdgeLauncher> logger)
        {
            _edgeDriverService = EdgeDriverService.CreateDefaultService(EdgeDriverPath);
            _edgeDriverService.HideCommandPromptWindow = true;

            _edgeOptions = new EdgeOptions();
            _edgeOptions.AddArgument("--remote-debugging-port=9222");
            _edgeOptions.AddArgument($"--user-data-dir={EdgeUserDataPath}");
            _edgeOptions.AddArgument("--profile-directory=Profile 1");
            
            _logger = logger;
        }

        public EdgeDriver GetEdgeDriver() => Driver!;

        public void StartLauncher()
        {
            Driver = new EdgeDriver(_edgeDriverService, _edgeOptions);
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
