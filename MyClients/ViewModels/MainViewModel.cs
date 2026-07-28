using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.BLL.Interfaces;
using MyClients.Constants;
using CommunityToolkit.Mvvm.Input;

namespace MyClients.ViewModels;

partial class MainViewModel : ObservableObject
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
		await Shell.Current.GoToAsync($"//{AppRoutes.DisciplinesPageRoute}");
	}
	
	[RelayCommand]
	public async Task StartTrainingAsync()
	{
		await Shell.Current.GoToAsync($"//{AppRoutes.TrainPageRoute}");
	}
}
