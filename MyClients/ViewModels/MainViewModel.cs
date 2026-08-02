using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

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
		var userEmail = Session.CurrentUserEmail;
		if (string.IsNullOrWhiteSpace(userEmail))
		{
			await LogoutAndRedirectToLoginPage("Ваша сесія закінчилась або користувача не знайдено. Будь ласка, увійдіть знову.");
			return;
		}

		try
		{
			var user = await _userService.GetUserByEmailAsync(userEmail);

			if (user == null)
			{
				await LogoutAndRedirectToLoginPage("Користувача не знайдено за вказаним email. Будь ласка, увійдіть знову.");
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
			await LogoutAndRedirectToLoginPage("Помилка формату email. Будь ласка, увійдіть знову.");
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

	private async Task LogoutAndRedirectToLoginPage(string logoutReason)
	{
		var navigationParameter = new Dictionary<string, object>
		{
			{ "LogoutReason", logoutReason }
		};
		
		ClearSession();
		await Shell.Current.GoToAsync($"//{AppRoutes.LogInPage}", navigationParameter);
	}
	
	private void ClearSession()
	{
		Session.CurrentUserEmail = string.Empty;
	}
}
