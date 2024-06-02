namespace SeleniumScraper.CommandsDomain.Abstract;

public interface ICommandProcessor
{
    IEnumerable<ICommand> Commands { get; }
    void AddCommand(ICommand command);
    void Process(int number);
    ICommand FindCommand(int id);
}
