using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SeleniumScraper.CommandsDomain;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.CommandsDomain.Commands;
using SeleniumScraper.CommandsDomain.Managers;
using SeleniumScraper.Services;
using SeleniumScraper.Services.Commands;

namespace SeleniumScraper
{
    public class DependenciesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddLogging(context =>
            {
                context.ClearProviders();
                context.AddNLog();
            });

            builder.Populate(services);

            builder.RegisterType<ProcessedData>().AsSelf().SingleInstance();

            // Selenium 
            builder.RegisterType<EdgeLauncher>().AsSelf().SingleInstance();
            builder.RegisterType<ConsoleUIService>().As<IUserInterfaceService>().SingleInstance();

            builder.RegisterType<StartLauncherCommand>().As<ICommand>().InstancePerLifetimeScope();
            builder.RegisterType<NavigateCommand>().As<ICommand>().InstancePerLifetimeScope();
            builder.RegisterType<SelectNewsPost>().As<ICommand>().InstancePerLifetimeScope();
            builder.RegisterType<DisplayParsedComments>().As<ICommand>().InstancePerLifetimeScope();
            builder.RegisterType<SaveDataCommand>().As<ICommand>().InstancePerLifetimeScope();
            builder.RegisterType<StopLauncherCommand>().As<ICommand>().InstancePerLifetimeScope();

            // Services
            builder.RegisterType<CommandManager>().As<ICommandManager>().SingleInstance();

            builder.Register(context =>
            {
                var commandManager = context.Resolve<ICommandManager>();
                var commands = context.Resolve<IEnumerable<ICommand>>();

                return new CommandProcessor(commandManager, commands);
            }).As<ICommandProcessor>().SingleInstance();

            builder.RegisterType<CommandService>().AsSelf().SingleInstance();
        }
    }
}
