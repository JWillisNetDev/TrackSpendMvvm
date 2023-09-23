using TrackSpendMvvm.Data.Interfaces;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Services;

public class TrackSpendService : ITrackSpendService
{
	private readonly IDataProvider _dp;

	public TrackSpendService(IDataProvider dataProvider)
	{
		_dp = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
	}

	public async Task AddMonthlyExpenseAsync(MonthlyExpense monthlyExpense)
	{
		_dp.MonthlyExpenses.Add(monthlyExpense);
		await _dp.SaveChangesAsync();
	}

	public async Task<IEnumerable<MonthlyExpense>> GetAllMonthlyExpensesAsync(int count = 0, int skip = 0)
	{
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(count));
		}
		
		if (count > 0 && skip < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(skip));
		}

		return count == 0
			? await _dp.MonthlyExpenses.GetAllAsync()
			: await _dp.MonthlyExpenses.GetAllAsync(count, skip);

	}

	public async Task<MonthlyExpense?> GetMonthlyExpenseAsync(string id)
	{
		return await _dp.MonthlyExpenses.GetAsync(id);
	}

	public async Task UpdateMonthlyExpenseAsync(MonthlyExpense monthlyExpense)
	{
		_dp.MonthlyExpenses.Update(monthlyExpense);
		await _dp.SaveChangesAsync();
	}

	public async Task<bool> RemoveMonthlyExpenseAsync(string id)
	{
		if (await _dp.MonthlyExpenses.GetAsync(id) is { } found
		    && _dp.MonthlyExpenses.Remove(found))
		{
			await _dp.SaveChangesAsync();
			return true;
		}
		return false;
	}
}