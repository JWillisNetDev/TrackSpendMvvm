using TrackSpendMvvm.Data.Interfaces;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Data;

public class DataSourceProvider : IDataProvider, IAsyncDisposable, IDisposable
{
	private readonly TrackSpendDbContext _db;

	public IDataSource<MonthlyExpense, string> MonthlyExpenses { get; }

	public DataSourceProvider(TrackSpendDbContext db)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		MonthlyExpenses = new DataSource<MonthlyExpense>(_db);
	}

	public void SaveChanges() => _db.SaveChanges();

	public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

	#region IDisposable
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_db.Dispose();
		}
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		await _db.DisposeAsync().ConfigureAwait(false);
	}
	#endregion
}