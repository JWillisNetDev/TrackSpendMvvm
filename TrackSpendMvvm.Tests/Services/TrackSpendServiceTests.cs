using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TrackSpendMvvm.Data;
using TrackSpendMvvm.Data.Models;
using TrackSpendMvvm.Services;
using TrackSpendMvvm.Tests.Extensions;

namespace TrackSpendMvvm.Tests.Services;

public class TrackSpendServiceTests
{
	private readonly AutoMocker _mocker;

	public TrackSpendServiceTests()
	{
		_mocker = new AutoMocker().WithInMemoryDatabase();
		using var db = _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContext();
		db.Database.EnsureCreated();
		db.SaveChanges();
	}

	[Fact]
	public async Task AddMonthlyExpense_AddsMonthlyExpense()
	{
		// Arrange
		var sut = _mocker.CreateInstance<TrackSpendService>();

		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		// Act
		await sut.AddMonthlyExpenseAsync(expense);

		// Asserts
		await using var db = await _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContextAsync();
		var actual = Assert.Single(db.MonthlyExpenses);
		Assert.False(string.IsNullOrWhiteSpace(actual.Id));
		Assert.Equal(expense.Title, actual.Title);
		Assert.Equal(expense.Description, actual.Description);
		Assert.Equal(expense.Amount, actual.Amount);
		Assert.Equal(expense.DayOfMonth, actual.DayOfMonth);

	}

	[Fact]
	public async Task GetAllMonthlyExpenses_GiveNothing_GetsAllMonthlyExpenses()
	{
		// Arrange
		var sut = _mocker.CreateInstance<TrackSpendService>();
		
		var db = await _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContextAsync();
		db.MonthlyExpenses.AddRange(
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
		await db.SaveChangesAsync();

		// Act
		var expenses = await sut.GetAllMonthlyExpensesAsync();

		// Assert
		Assert.Equal(3, expenses.Count);
	}

	[Fact]
	public async Task GetMonthlyExpense_GivenExistingMonthlyExpenseId_GetsMonthlyExpense()
	{
		// Arrange
		var sut = _mocker.CreateInstance<TrackSpendService>();

		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var db = await _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContextAsync();
		db.MonthlyExpenses.Add(expense);
		await db.SaveChangesAsync();

		// Act
		var actual = await sut.GetMonthlyExpenseAsync(expense.Id);

		// Assert
		var expected = Assert.Single(db.MonthlyExpenses);
		Assert.NotNull(actual);
		Assert.Equal(expected.Id, actual.Id);
		Assert.Equal(expected.Title, actual.Title);
		Assert.Equal(expected.Description, actual.Description);
		Assert.Equal(expected.Amount, actual.Amount);
		Assert.Equal(expected.DayOfMonth, actual.DayOfMonth);
	}

	[Fact]
	public async Task UpdateMonthlyExpense_GivenExistingMonthlyExpense_UpdatesGivenMonthlyExpense()
	{
		// Arrange
		const string expectedTitle = "Updated title";

		var sut = _mocker.CreateInstance<TrackSpendService>();

		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var db = await _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContextAsync();
		db.MonthlyExpenses.Add(expense);
		await db.SaveChangesAsync();

		// Act
		expense.Title = expectedTitle;
		await sut.UpdateMonthlyExpenseAsync(expense);

		// Assert
		var actual = Assert.Single(db.MonthlyExpenses);
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
		var sut = _mocker.CreateInstance<TrackSpendService>();

		var expense = new MonthlyExpense
		{
			Title = "Test title",
			Description = "Test description",
			Amount = 42.00M,
			DayOfMonth = 10,
		};

		var db = await _mocker.Get<IDbContextFactory<TrackSpendDbContext>>().CreateDbContextAsync();
		db.MonthlyExpenses.Add(expense);
		await db.SaveChangesAsync();

		// Act
		var removed = await sut.RemoveMonthlyExpenseAsync(expense.Id);

		// Assert
		Assert.True(removed);
		Assert.Empty(db.MonthlyExpenses);
	}
}