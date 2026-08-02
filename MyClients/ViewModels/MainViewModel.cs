using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.BLL.Interfaces;
using MyClients.Constants;
using CommunityToolkit.Mvvm.Input;

namespace MyClients.ViewModels;

public partial class MainViewModel : ObservableObject
{
	private readonly IUserService _userService;

	public MainViewModel(IUserService userService)
	{
		this._userService = userService;
	}
	
	[ObservableProperty]
	private string _userName = String.Empty;

	public async Task LoadUserNameAsync()
	{
		var user = await _userService.GetUserByEmailAsync(Session.CurrentUserEmail);
		UserName = user?.Name ?? string.Empty;
	}

	[RelayCommand]
	public async Task GoToDisciplinesAsync()
	{
		try
		{
			await Shell.Current.GoToAsync($"//{AppRoutes.DisciplinesPageRoute}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"Navigation to {AppRoutes.DisciplinesPageRoute} error: {e}");
			if (Shell.Current != null)
			{
				await Shell.Current.DisplayAlertAsync(
					title: "Woops...",
					message: "We couldn't show you disciplines. Try again!",
					cancel: "OK");
			}
		}
	}
	
	[RelayCommand]
	public async Task StartTrainingAsync()
	{
		try
		{
			await Shell.Current.GoToAsync($"//{AppRoutes.TrainPageRoute}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"Navigation to {AppRoutes.TrainPageRoute} error: {e}");
			if (Shell.Current != null)
			{
				await Shell.Current.DisplayAlertAsync(
					title: "Woops...",
					message: "We couldn't start your training. Try again!",
					cancel: "OK");
			}
		}
	}
}
