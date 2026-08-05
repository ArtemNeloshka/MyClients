using CommunityToolkit.Mvvm.Input;
using MyClients.Domain.Constants;
using MyClients.Views;

namespace MyClients.ViewModels;

[QueryProperty(nameof(AlertMessage), Navigation.AlertMessageKey)]
[QueryProperty(nameof(IsErrorAlert), Navigation.IsErrorKey)]
public partial class StatisticsPageViewModel : BaseViewModel
{
	private string _alertMessage;

	public string AlertMessage
	{
		get => _alertMessage;
		set
		{
			_alertMessage = value;
			if (string.IsNullOrEmpty(value))
			{
				Shell.Current.DisplayAlertAsync(
					IsErrorAlert ? "Error" : "Success",
					value,
					"Ok");

				_alertMessage = string.Empty;
			}
		}
	}
	
	public bool IsErrorAlert { get; set; }
	
	[RelayCommand]
	private async Task OpenWorkoutsArchivePageAsync()
	{
		await Shell.Current.GoToAsync(nameof(WorkoutsArchivePage));
	}
}
