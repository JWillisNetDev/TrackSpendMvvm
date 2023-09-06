using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data;

namespace TrackSpendMvvm.Tests.Doubles;

internal class TestTrackSpendDbContextFactory : IDbContextFactory<TrackSpendDbContext>, IDisposable
{
	private readonly SqliteConnection _connection;
	private readonly DbContextOptions<TrackSpendDbContext> _options;

	public TestTrackSpendDbContextFactory()
	{
		_connection = new SqliteConnection("Data Source=:memory:;");
		_connection.Open();

		_options = new DbContextOptionsBuilder<TrackSpendDbContext>()
			.UseSqlite(_connection)
			.Options;
	}

	public TrackSpendDbContext CreateDbContext() => new(_options);

	public void Dispose() => _connection.Dispose();
}