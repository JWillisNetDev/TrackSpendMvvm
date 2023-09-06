using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TrackSpendMvvm.ViewModels;

public abstract partial class BaseViewModel : ObservableRecipient, IViewModel
{
	public virtual async Task OnInitializedAsync()
	{
		await Loaded().ConfigureAwait(true);
	}
	public virtual void NotifyStateChanged() => OnPropertyChanged((string?)null);

	[RelayCommand]
	public virtual async Task Loaded()
	{
		await Task.CompletedTask.ConfigureAwait(false);
	}
}