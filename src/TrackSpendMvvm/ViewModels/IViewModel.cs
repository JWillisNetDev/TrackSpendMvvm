using System.ComponentModel;

namespace TrackSpendMvvm.ViewModels;

public interface IViewModel : INotifyPropertyChanged
{
	Task OnInitializedAsync();
	Task Loaded();
}