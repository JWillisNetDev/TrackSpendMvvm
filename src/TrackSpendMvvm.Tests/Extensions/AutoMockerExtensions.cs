using TrackSpendMvvm.Data.Interfaces;
using TrackSpendMvvm.Data.Models;

namespace TrackSpendMvvm.Tests.Extensions;

internal static class AutoMockerExtensions
{
	public static AutoMocker WithMonthlyExpenseProvider(this AutoMocker mocker, out IDataSource<MonthlyExpense, string> dataSource, params MonthlyExpense[] seedData)
	{
		dataSource = new TestDataSource<MonthlyExpense, string>(x => x.Id, seedData.Select(Util.DeepCopy).ToDictionary(x => x.Id));
		
		mocker
			.Setup<IDataProvider, IDataSource<MonthlyExpense, string>>(x => x.MonthlyExpenses)
			.Returns(dataSource);
		
		return mocker;
	}
	public static AutoMocker WithMonthlyExpenseProvider(this AutoMocker mocker, params MonthlyExpense[] seedData)
	{
		mocker.WithMonthlyExpenseProvider(out var _, seedData);

		return mocker;
	}

}