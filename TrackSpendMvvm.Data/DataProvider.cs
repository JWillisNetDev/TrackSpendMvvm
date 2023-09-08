using System.Diagnostics;
using TrackSpendMvvm.Data.Interfaces;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Data;

public sealed class DataProvider : IDataProvider, IAsyncDisposable, IDisposable
{
	private readonly TrackSpendDbContext _db;

	public IDataSource<MonthlyExpense, string> MonthlyExpenses { get; }

	public DataProvider(TrackSpendDbContext db)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		MonthlyExpenses = new DataSource<MonthlyExpense>(_db);
	}

	public void SaveChanges() => _db.SaveChanges();

	public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

	#region IDisposable
	private bool _disposed;

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
		{
			return;
		}

		await _db.DisposeAsync();
		_disposed = true;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_db.Dispose();
		_disposed = true;
	}
	#endregion
}