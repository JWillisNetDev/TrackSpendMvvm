using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data;
using TrackSpendMvvm.Tests.Doubles;

namespace TrackSpendMvvm.Tests.Extensions;

internal static class AutoMockerExtensions
{
	public static AutoMocker WithInMemoryDatabase(this AutoMocker mocker)
	{
		mocker.With<IDbContextFactory<TrackSpendDbContext>, TestTrackSpendDbContextFactory>();
		return mocker;
	}
}