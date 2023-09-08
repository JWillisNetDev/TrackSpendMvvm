using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Services;

public interface ITrackSpendService
{
	Task AddMonthlyExpenseAsync(MonthlyExpense monthlyExpense);
	Task<IEnumerable<MonthlyExpense>> GetAllMonthlyExpensesAsync(int count = 0, int skip = 0);
	Task<MonthlyExpense?> GetMonthlyExpenseAsync(string id);
	Task UpdateMonthlyExpenseAsync(MonthlyExpense monthlyExpense);
	Task<bool> RemoveMonthlyExpenseAsync(string id);
}