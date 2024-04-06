using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain;

public class CommandProcessor : ICommandProcessor
{
    private readonly ICommandManager _commandManager;

    public IEnumerable<ICommand> Commands => _commandManager.Read<ICommand>();

    public CommandProcessor(ICommandManager commandManager, IEnumerable<ICommand> commandsList)
    {
        _commandManager = commandManager;
        
        foreach (var cmd in commandsList)
        {
            _commandManager.Write(cmd);
        }   
    }

    public void AddCommand(ICommand command)
    {
        _commandManager.Write(command);
    }

    public ICommand FindCommand(int id)
    {
        if (_commandManager == null)
        {
            throw new Exception("Command manager isn't set");
        }
        else
        {
            var result = _commandManager.Read<ICommand>().FirstOrDefault(command => command.Id == id);
            return result!;
        }

    }

    public void Process(int number)
    {
        if (_commandManager.Read<ICommand>().Any(cmd => cmd.Id == number))
        {
            var command = _commandManager.Read<ICommand>().First(cmd => cmd.Id == number);
            command.ExecuteCommand();
        }
    }
}
