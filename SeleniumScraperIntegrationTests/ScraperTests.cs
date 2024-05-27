using Autofac;
using FluentAssertions;
using Moq;
using SeleniumScraper;
using SeleniumScraper.CommandsDomain.Commands;
using SeleniumScraper.Services;
using SeleniumScraper.Services.Commands;


namespace SeleniumScraperIntegrationTests
{
    public class Tests: IDisposable
    {
        private IContainer Container;
        private List<string> CommandOutputMessages = new();

        private IContainer ConfigureDependencies()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DependenciesModule>();

            containerBuilder.Register(context =>
            {
                var testUIService = new Mock<IUserInterfaceService>();
                testUIService.Setup(x => x.DisplayMessage(Capture.In(CommandOutputMessages)));
                
                testUIService.Setup(x => x.ReadInput(It.IsAny<string>())).Returns(string.Empty);
                testUIService.Setup(x => x.ReadInput("Enter number of comments to read: ")).Returns("10");

                return testUIService.Object;
            }).As<IUserInterfaceService>();

            return containerBuilder.Build();
        }

        [SetUp]
        public void Setup()
        {
            Container = ConfigureDependencies();
        }

        [Test]
        public void LaunchBrowserAndReadFirstTenTweets_ReturnsFirstTestTweets()
        {
            var manager = Container.Resolve<CommandService>();
            var commandProcessor = manager.CommandProcessor;

            //Start web driver
            var startLauncherCommand = commandProcessor.FindCommand(1);
            startLauncherCommand.ExecuteCommand();

            //Navigate to test url
            var navigateCommand = (NavigateCommand)commandProcessor.FindCommand(2);
            navigateCommand.UrlToNavigate = "https://twitter.com/borisjohnson/status/1789204110417260575";
            navigateCommand.ExecuteCommand();

            //Wait for page is loaded
            Thread.Sleep(2000);

            //Get first 10 comments
            var getPostComments = commandProcessor.FindCommand(3);
            getPostComments.ExecuteCommand();

            //Stop and exit web driver
            var stopLauncherCommand = commandProcessor.FindCommand(0);
            stopLauncherCommand.ExecuteCommand();

            //Check output messages for added 10 comments
            CommandOutputMessages.Should().NotBeEmpty();
        }

        public void Dispose()
        {
            Container?.Dispose();
        }
    }
}