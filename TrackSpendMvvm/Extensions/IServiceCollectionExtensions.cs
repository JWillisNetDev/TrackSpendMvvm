using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data;
using TrackSpendMvvm.Services;
using TrackSpendMvvm.ViewModels;

namespace TrackSpendMvvm.Extensions;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddViewModels(this IServiceCollection collection)
	{
		collection.AddTransient<MonthlyExpenseListViewModel>();
		return collection;
	}

	public static IServiceCollection AddTrackSpendService(this IServiceCollection collection, IConfiguration configuration)
	{
		collection.AddDbContextFactory<TrackSpendDbContext>(builder =>
		{
			builder.UseSqlServer(configuration.GetConnectionString("default"));
		});

		collection.AddScoped<ITrackSpendService, TrackSpendService>();
		
		return collection;
	}
}