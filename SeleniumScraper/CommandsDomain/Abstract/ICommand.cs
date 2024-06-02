using SeleniumScraper.Services;

namespace SeleniumScraper.CommandsDomain.Abstract;

public interface ICommand : IEntity
{
    IUserInterfaceService UserInterfaceService { get; }
    string DisplayCommandName { get; }
    void ExecuteCommand();
}
