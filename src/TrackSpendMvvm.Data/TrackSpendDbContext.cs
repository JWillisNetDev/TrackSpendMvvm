using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Data;

public class TrackSpendDbContext : DbContext
{
	public TrackSpendDbContext(DbContextOptions<TrackSpendDbContext> options)
		: base(options)
	{
	}

	public DbSet<MonthlyExpense> MonthlyExpenses => Set<MonthlyExpense>();
}