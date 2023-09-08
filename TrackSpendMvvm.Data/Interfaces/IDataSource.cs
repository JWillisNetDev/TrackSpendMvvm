using System.Linq.Expressions;

namespace TrackSpendMvvm.Data.Interfaces;

public interface IDataSource<TEntity, in TKey> : IEnumerable<TEntity>
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    Task<TEntity?> GetAsync(TKey key, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(int count = 0, int skip = 0, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    void Add(TEntity entity);
    void Update(TEntity entity);
    bool Remove(TEntity entity);
}
