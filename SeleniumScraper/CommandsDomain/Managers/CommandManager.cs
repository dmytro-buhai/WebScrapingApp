using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain.Managers;

public class CommandManager : ICommandManager
{
    private readonly Dictionary<int, ICommand> _commandSet = [];

    public IEnumerable<T> Read<T>()
              where T : ICommand
    {
        return _commandSet.Values
            .Where(v => v is T)
            .Cast<T>()
            .AsEnumerable();
    }

    public void Write<T>(params T[] data)
        where T : ICommand
    {
        foreach (var command in data)
        {
            _commandSet.TryAdd(command.Id, command);
        }
    }
}
