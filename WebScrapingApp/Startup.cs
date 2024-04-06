using Autofac;
using Microsoft.Extensions.Logging;
using SeleniumScraper;
using SeleniumScraper.Services.Commands;

namespace WebScrapingApp;

public class Startup
{
    private static IContainer Container = ConfigureDependencies();

    private static IContainer ConfigureDependencies()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule<DependenciesModule>();
        return containerBuilder.Build();
    }

    public static void StartApplication()
    {
        var manager = Container.Resolve<CommandService>();
        manager.Start();
    }
}
