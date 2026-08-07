using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Views;

namespace MyClients.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
	private readonly IUserService _userService;

	public LoginViewModel(IUserService userService)
	{
		this._userService = userService;
	}

	[ObservableProperty]
	private string _email = string.Empty;
	[ObservableProperty]
	private string _password = string.Empty;
	[ObservableProperty]
	private string _emailPlaceholder = "Enter your email...";
	[ObservableProperty]
	private Color _emailPlaceholderColor = Colors.Black;
	[ObservableProperty]
	private string _passwordPlaceholder = "Enter your password...";
	[ObservableProperty]
	private Color _passwordPlaceholderColor = Colors.Black;

	private string _alertMessage;

	public string AlertMessage
	{
		get => _alertMessage;
		set
		{
			_alertMessage = value;
			if (string.IsNullOrEmpty(value)) return;
			Shell.Current.DisplayAlertAsync(
				IsErrorAlert ? "Error" : "Success",
				value,
				"Ok");

			_alertMessage = string.Empty;
		}
	}
	
	public bool IsErrorAlert { get; set; }
	
	private async Task<(bool Success, string? ErrorMessage)> LogInUserAsync()
	{
		if (string.IsNullOrWhiteSpace(this.Email))
		{
			return (false, ErrorPlaceholders.EmailIsEmpty);
		}

		if (!ValidationRules.IsValidEmail(this.Email))
		{
			return (false, ErrorPlaceholders.InvalidEmail);
		}

		if (string.IsNullOrWhiteSpace(this.Password))
		{
			return (false, ErrorPlaceholders.LogInPasswordIsEmpty);
		}

		if (this.Password.Length < ValidationRules.MinPasswordLength)
		{
			return (false, ErrorPlaceholders.PasswordIsShort);
		}

		try
		{
			var result = await _userService.LoginUserAsync(this.Email, this.Password);
			if (result is not { Success: true, userId: not null })
				return result.ErrorMessage switch
				{
					ErrorMessages.UserNotFound => (false, ErrorPlaceholders.LogInEmailNotFound),
					ErrorMessages.PasswordIncorrect => (false, ErrorPlaceholders.PasswordIncorrect),
					_ => (false, result.ErrorMessage)
				};
			
			Session.CurrentUserEmail = this.Email;
			Session.CurrentUserId = result.userId;
			return (true, null);

		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return (false, "Unexpected error appeared. Try later!");
		}
	}

	[RelayCommand]
	private async Task LogInAsync()
	{
		var result = await this.LogInUserAsync();

		if (result.Success)
		{
			Application.Current.MainPage = new AppShell();
			return;
		}

		switch (result.ErrorMessage)
		{
			case ErrorPlaceholders.EmailIsEmpty:
				SetEmailError(ErrorPlaceholders.EmailIsEmpty);
				break;
			
			case ErrorPlaceholders.LogInPasswordIsEmpty:
				SetPasswordError(ErrorPlaceholders.LogInPasswordIsEmpty);
				break;
			
			case ErrorPlaceholders.InvalidEmail:
				SetEmailError(ErrorPlaceholders.InvalidEmail);
				break;
			
			case ErrorPlaceholders.LogInEmailNotFound:
				SetEmailError(ErrorPlaceholders.LogInEmailNotFound);
				break;
			
			case ErrorPlaceholders.PasswordIsShort:
				SetPasswordError(ErrorPlaceholders.PasswordIsShort);
				break;
			
			case ErrorPlaceholders.PasswordIncorrect:
				SetPasswordError(ErrorPlaceholders.PasswordIncorrect);
				break;
			
			default:
				SetEmailError(result.ErrorMessage ?? "Unknown error");
				break;
		}
	}

	[RelayCommand]
	private void NavigateToRegisterPage()
	{
		var registrationPage = Application.Current.Handler?.MauiContext?.Services.GetService<RegistrationPage>();
		Application.Current.MainPage = registrationPage;
	}
	
	private void SetEmailError(string placeholder)
	{
		this.Email = string.Empty;
		this.EmailPlaceholder = placeholder;
		this.EmailPlaceholderColor = Colors.Red;
	}
	
	private void SetPasswordError(string placeholder)
	{
		this.Password = string.Empty;
		this.PasswordPlaceholder = placeholder;
		this.PasswordPlaceholderColor = Colors.Red;
	}

	partial void OnEmailChanged(string value)
	{
		if (this.EmailPlaceholderColor == Colors.Red)
		{
			this.EmailPlaceholderColor = Colors.Black;
			this.EmailPlaceholder = "Enter your email...";
		}
	}
	
	partial void OnPasswordChanged(string value)
	{
		if (this.PasswordPlaceholderColor == Colors.Red)
		{
			this.PasswordPlaceholderColor = Colors.Black;
			this.PasswordPlaceholder = "Enter your password...";
		}
	}
}
