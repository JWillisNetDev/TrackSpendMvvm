using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using TrackSpendMvvm.Data.Models;
using TrackSpendMvvm.Services;

namespace TrackSpendMvvm.ViewModels;

public partial class MonthlyExpenseListViewModel : BaseViewModel
{
	private readonly ITrackSpendService _trackSpendService;

	[ObservableProperty]
	private ObservableCollection<MonthlyExpense>? _items;

	public MonthlyExpenseListViewModel(ITrackSpendService trackSpendService)
	{
		_trackSpendService = trackSpendService ?? throw new ArgumentNullException(nameof(trackSpendService));
	}

	public override async Task OnInitializedAsync()
	{
		await Task.Delay(TimeSpan.FromSeconds(3));
		Items = new ObservableCollection<MonthlyExpense>();
		foreach (var item in await _trackSpendService.GetAllMonthlyExpensesAsync())
		{
			Items.Add(item);
		}
		await base.OnInitializedAsync();
	}

	[RelayCommand]
	private void AddItem()
	{
		Items.Add(new MonthlyExpense());
	}

	[RelayCommand]
	private void RemoveItem(MonthlyExpense item)
	{
		Items.Remove(item);
	}
}