using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.Views;

namespace MyClients.ViewModels;

public partial class StatisticsPageViewModel : ObservableObject
{
	[RelayCommand]
	private async Task OpenWorkoutsArchivePageAsync()
	{
		await Shell.Current.GoToAsync(nameof(WorkoutsArchivePage));
	}
}
