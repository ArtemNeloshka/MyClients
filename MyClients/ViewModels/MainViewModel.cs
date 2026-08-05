using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

namespace MyClients.ViewModels;

public partial class MainViewModel : BaseViewModel
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
		var userEmail = Session.CurrentUserEmail;
		if (string.IsNullOrWhiteSpace(userEmail))
		{
			await GoBackWithAlertAsync(
				message: "Ваша сесія закінчилась або користувача не знайдено. Будь ласка, увійдіть знову.",
				pagePath: $"//{AppRoutes.LoginPage}",
				isError: true);
			return;
		}

		try
		{
			var user = await _userService.GetUserByEmailAsync(userEmail);

			if (user == null)
			{
				await GoBackWithAlertAsync(
					message: "Користувача не знайдено за вказаним email. Будь ласка, увійдіть знову.",
					pagePath: $"//{AppRoutes.LoginPage}",
					isError: true);
			}
			else
			{
				UserName = string.IsNullOrWhiteSpace(user.Name) 
					? "ім'я не вказано" 
					: user.Name;
			}
		}
		catch (ArgumentException e)
		{
			await GoBackWithAlertAsync(
				message: "Помилка формату email. Будь ласка, увійдіть знову.",
				pagePath: $"//AppRoutes.LoginPage",
				isError: true);
		}
		catch (Exception)
		{
			UserName = "Помилка завантаження";
		}
	}

	[RelayCommand]
	private async Task NavigateToDisciplinesAsync()
	{
		await Shell.Current.GoToAsync($"//{AppRoutes.DisciplinesPage}");
		Console.WriteLine("Navigated to disciplines from main page."); 
	}
	
	[RelayCommand]
	private async Task StartNewTrainingAsync()
	{
		await Shell.Current.GoToAsync($"//{AppRoutes.TrainPage}");
		Console.WriteLine("Started training from main page.");
	}
}
