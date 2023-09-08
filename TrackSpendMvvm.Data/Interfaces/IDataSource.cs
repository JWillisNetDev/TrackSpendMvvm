using System.Linq.Expressions;

namespace TrackSpendMvvm.Data.Interfaces;

public interface IDataSource<TEntity, in TKey> : IEnumerable<TEntity>
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    public Task<TEntity?> GetAsync(TKey key, CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>> GetAllAsync(int count = 0, int skip = 0, CancellationToken cancellationToken = default);
    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public bool Remove(TEntity entity);
}