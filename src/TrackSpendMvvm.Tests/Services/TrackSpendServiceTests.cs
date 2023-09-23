using Moq;
using TrackSpendMvvm.Data.Interfaces;
using TrackSpendMvvm.Data.Models;
using TrackSpendMvvm.Services;
using TrackSpendMvvm.Tests.Extensions;

namespace TrackSpendMvvm.Tests.Services;

public class TrackSpendServiceTests
{
	[Fact]
	public async Task AddMonthlyExpense_AddsMonthlyExpense()
	{
		// Arrange
		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var mocker = new AutoMocker().WithMonthlyExpenseProvider(out var dataSource);

		var sut = mocker.CreateInstance<TrackSpendService>();

		// Act
		await sut.AddMonthlyExpenseAsync(expense);

		// Asserts
		mocker.Verify<IDataProvider>(x => x.SaveChangesAsync(), times: Times.Once);
		var actual = Assert.Single(dataSource);
		Assert.Equal(expense.Id, actual.Id);
		Assert.Equal(expense.Title, actual.Title);
		Assert.Equal(expense.Description, actual.Description);
		Assert.Equal(expense.Amount, actual.Amount);
		Assert.Equal(expense.DayOfMonth, actual.DayOfMonth);
	}

	[Fact]
	public async Task GetAllMonthlyExpenses_GiveNothing_GetsAllMonthlyExpenses()
	{
		// Arrange
		var mocker = new AutoMocker().WithMonthlyExpenseProvider(
			new MonthlyExpense
			{
				Title = "Test 1",
				Description = "Description 1",
				Amount = 1.01M,
				DayOfMonth = 10,
			},
			new MonthlyExpense
			{
				Title = "Test 2",
				Description = "Description 2",
				Amount = 2.20M,
				DayOfMonth = 20,
			},
			new MonthlyExpense
			{
				Title = "Test 3",
				Description = "Description 3",
				Amount = 300.42M,
				DayOfMonth = 30,
			});

		var sut = mocker.CreateInstance<TrackSpendService>();

		// Act
		var expenses = await sut.GetAllMonthlyExpensesAsync();

		// Assert
		Assert.Equal(3, expenses.Count());
	}

	[Fact]
	public async Task GetMonthlyExpense_GivenExistingMonthlyExpenseId_GetsMonthlyExpense()
	{
		// Arrange
		string guid = Guid.NewGuid().ToString();
		var expense = new MonthlyExpense
		{
			Id = guid,
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var mocker = new AutoMocker().WithMonthlyExpenseProvider(expense);

		var sut = mocker.CreateInstance<TrackSpendService>();

		// Act
		var actual = await sut.GetMonthlyExpenseAsync(guid);

		// Assert
		Assert.NotNull(actual);
		Assert.False(ReferenceEquals(actual, expense));
		Assert.Equal(expense.Id, actual.Id);
		Assert.Equal(expense.Title, actual.Title);
		Assert.Equal(expense.Description, actual.Description);
		Assert.Equal(expense.Amount, actual.Amount);
		Assert.Equal(expense.DayOfMonth, actual.DayOfMonth);
	}

	[Fact]
	public async Task UpdateMonthlyExpense_GivenExistingMonthlyExpense_UpdatesGivenMonthlyExpense()
	{
		// Arrange
		const string expectedTitle = "Updated title";

		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var mocker = new AutoMocker().WithMonthlyExpenseProvider(out var dataSource, expense);

		var sut = mocker.CreateInstance<TrackSpendService>();

		// Act
		expense.Title = expectedTitle;
		await sut.UpdateMonthlyExpenseAsync(expense);

		// Assert
		mocker.Verify<IDataProvider>(x => x.SaveChangesAsync(), times: Times.Once);
		var actual = Assert.Single(dataSource);
		Assert.Equal(expense.Id, actual.Id);
		Assert.Equal(expectedTitle, actual.Title);
		Assert.Equal(expense.Description, actual.Description);
		Assert.Equal(expense.Amount, actual.Amount);
		Assert.Equal(expense.DayOfMonth, actual.DayOfMonth);
	}

	[Fact]
	public async Task RemoveMonthlyExpense_GivenExistingMonthlyExpenseId_RemovesGivenMonthlyExpense()
	{
		// Arrange
		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var mocker = new AutoMocker().WithMonthlyExpenseProvider(out var dataSource, expense);

		var sut = mocker.CreateInstance<TrackSpendService>();

		// Act
		var removed = await sut.RemoveMonthlyExpenseAsync(expense.Id);

		// Assert
		mocker.Verify<IDataProvider>(x => x.SaveChangesAsync(), times: Times.Once);
		Assert.True(removed);
		Assert.Empty(dataSource);
	}
}