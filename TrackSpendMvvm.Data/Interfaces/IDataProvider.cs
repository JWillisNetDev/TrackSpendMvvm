using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Data.Interfaces;

public interface IDataProvider
{
    IDataSource<MonthlyExpense, string> MonthlyExpenses { get; }

    void SaveChanges();
    Task SaveChangesAsync();
}