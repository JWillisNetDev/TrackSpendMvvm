using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Services;

public class TrackSpendService : ITrackSpendService
{
	private readonly IDbContextFactory<TrackSpendDbContext> _dbFactory;

	public TrackSpendService(IDbContextFactory<TrackSpendDbContext> dbFactory)
	{
		_dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
	}

	public async Task AddMonthlyExpenseAsync(MonthlyExpense monthlyExpense)
	{
		await using var db = await _dbFactory.CreateDbContextAsync();

		await db.MonthlyExpenses.AddAsync(monthlyExpense);
		await db.SaveChangesAsync();
	}

	public async Task<IReadOnlyCollection<MonthlyExpense>> GetAllMonthlyExpensesAsync(int count = 0)
	{
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(count));
		}

		await using var db = await _dbFactory.CreateDbContextAsync();

		return count == 0
			? await db.MonthlyExpenses.ToArrayAsync()
			: await db.MonthlyExpenses.Take(count).ToArrayAsync();
	}

	public async Task<MonthlyExpense?> GetMonthlyExpenseAsync(string id)
	{
		await using var db = await _dbFactory.CreateDbContextAsync();

		return await db.MonthlyExpenses.FindAsync(id);
	}

	public async Task UpdateMonthlyExpenseAsync(MonthlyExpense monthlyExpense)
	{
		await using var db = await _dbFactory.CreateDbContextAsync();

		db.MonthlyExpenses.Update(monthlyExpense);
		await db.SaveChangesAsync();
	}

	public async Task<bool> RemoveMonthlyExpenseAsync(string id)
	{
		await using var db = await _dbFactory.CreateDbContextAsync();
		
		if (await db.MonthlyExpenses.FindAsync(id) is not { } found)
		{
			return false;
		}

		db.MonthlyExpenses.Remove(found);
		return await db.SaveChangesAsync() > 0;
	}
}