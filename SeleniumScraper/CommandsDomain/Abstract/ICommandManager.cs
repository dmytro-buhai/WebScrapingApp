namespace SeleniumScraper.CommandsDomain.Abstract
{
    public interface ICommandManager
    {
        IEnumerable<T> Read<T>() where T : ICommand;
        void Write<T>(params T[] data) where T : ICommand;
    }
}