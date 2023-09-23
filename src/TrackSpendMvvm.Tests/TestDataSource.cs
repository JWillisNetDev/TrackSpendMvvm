using System.Collections;
using System.Linq.Expressions;
using TrackSpendMvvm.Data.Interfaces;

namespace TrackSpendMvvm.Tests;

internal class TestDataSource<TEntity, TKey>  : IDataSource<TEntity, TKey>
	where TEntity : class, new()
	where TKey : IEquatable<TKey>
{
	private readonly Func<TEntity, TKey> _keySelector;
	public Dictionary<TKey, TEntity> Data { get; set; }

	public TestDataSource(Func<TEntity, TKey> keySelector, Dictionary<TKey, TEntity>? data = null)
	{
		_keySelector = keySelector;
		Data = data ?? new Dictionary<TKey, TEntity>();
	}

	public Task<TEntity?> GetAsync(TKey key, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(Data[key])!;
	}

	public Task<IEnumerable<TEntity>> GetAllAsync(int count = 0, int skip = 0, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(Data.Values.AsEnumerable());
	}

	public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		var lambda = predicate.Compile();
		return Task.FromResult(Data.Values.FirstOrDefault(lambda));
	}

	public Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		var lambda = predicate.Compile();
		return Task.FromResult(Data.Values.Where(lambda));
	}

	public void Add(TEntity entity)
	{
		TEntity clone = Util.DeepCopy(entity);
		Data.Add(_keySelector(clone), clone);
	}

	public void Update(TEntity entity)
	{
		TEntity clone = Util.DeepCopy(entity);
		Data[_keySelector(clone)] = clone;
	}

	public bool Remove(TEntity entity)
	{
		return Data.Remove(_keySelector(entity));
	}

	public IEnumerator<TEntity> GetEnumerator() => Data.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}