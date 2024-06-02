using SeleniumScraper.CommandsDomain.Abstract;

namespace SeleniumScraper.CommandsDomain.Managers;

public class EntityManager : IEntityManager
{
    private readonly Dictionary<int, IEntity> _entitySet = [];

    public IEnumerable<T> Read<T>()
        where T : IEntity
    {
        return _entitySet.Values
            .Where(v => v is T)
            .Cast<T>()
            .AsEnumerable();
    }

    public void Write<T>(params T[] data)
        where T : IEntity
    {
        foreach (var entity in data)
        {
            _entitySet.TryAdd(entity.Id, entity);
        }
    }
}
