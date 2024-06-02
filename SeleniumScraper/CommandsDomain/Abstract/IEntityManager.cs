namespace SeleniumScraper.CommandsDomain.Abstract
{
    public interface IEntityManager
    {
        IEnumerable<T> Read<T>() where T : IEntity;
        void Write<T>(params T[] data) where T : IEntity;
    }
}
