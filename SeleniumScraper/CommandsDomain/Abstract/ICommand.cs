namespace SeleniumScraper.CommandsDomain.Abstract;

public interface ICommand : IEntity
{
    string DisplayCommandName { get; }
    void ExecuteCommand();
}
