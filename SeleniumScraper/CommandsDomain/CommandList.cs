using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.CommandsDomain.Commands;
using SeleniumScraper.CommandsDomain.Managers;

namespace SeleniumScraper.CommandsDomain;

public class CommandList
{
    public List<ICommand> commands = new List<ICommand>();

    static EntityManager entityManager = new EntityManager();
    //static IEnumerable<Func<ICommand>> commandsList = GetByBaseType<EdgeCommand>();

    public CommandList()
    {
        var commandsList = GetByBaseType<EdgeCommand>();

        var cmds = commandsList.Select(x => x()).ToList<ICommand>();

        commands.AddRange(cmds);
        //commands.Add(startGameCommand);
    }

    static IEnumerable<Func<ICommand>> GetByBaseType<T>()
    {
        var assemblies = typeof(T).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(T)) && t.GetInterface(nameof(ICommand)) != null);


        // We assume t has a parameterless constructor
        return assemblies.Select(t => (Func<ICommand>)(() => (ICommand)Activator.CreateInstance(t)));
    }

}
