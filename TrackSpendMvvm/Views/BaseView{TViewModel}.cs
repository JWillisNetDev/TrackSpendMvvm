using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using TrackSpendMvvm.ViewModels;

namespace TrackSpendMvvm.Views;

public class BaseView<TViewModel> : ComponentBase
    where TViewModel : class, IViewModel
{
    [Inject, NotNull]
    protected TViewModel ViewModel { get; set; } = null!;

    protected override void OnInitialized()
    {
        ViewModel.PropertyChanged += (_, _) => StateHasChanged();
        base.OnInitialized();
    }

    protected override Task OnInitializedAsync() => ViewModel.OnInitializedAsync();
}