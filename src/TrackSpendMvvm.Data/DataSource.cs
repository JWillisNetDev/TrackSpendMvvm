using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data.Interfaces;

namespace TrackSpendMvvm.Data;

public class DataSource<TEntity> : IDataSource<TEntity, string>
	where TEntity : class
{
	internal readonly TrackSpendDbContext Context;
	internal DbSet<TEntity> Set => Context.Set<TEntity>();

	public DataSource(TrackSpendDbContext context)
	{
		Context = context;
	}

	public async Task<TEntity?> GetAsync(string key, CancellationToken cancellationToken = default)
	{
		return await Set.FindAsync(key, cancellationToken);
	}

	public async Task<IEnumerable<TEntity>> GetAllAsync(int count = 0, int skip = 0, CancellationToken cancellationToken = default)
	{
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(count));
		}

		return count == 0
			? await Set.ToArrayAsync(cancellationToken)
			: await Set.Skip(skip).Take(count).ToArrayAsync(cancellationToken);
	}

	public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await Set.FirstOrDefaultAsync(predicate, cancellationToken);
	}

	public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await Set.Where(predicate).ToArrayAsync(cancellationToken);
	}

	public void Add(TEntity entity)
	{
		Set.Add(entity);
	}

	public void Update(TEntity entity)
	{
		Set.Entry(entity).State = EntityState.Modified;
	}

	public bool Remove(TEntity entity)
	{
		var entry = Set.Remove(entity);
		return entry.State == EntityState.Deleted;
	}

	public IEnumerator<TEntity> GetEnumerator() => Set.AsEnumerable().GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}