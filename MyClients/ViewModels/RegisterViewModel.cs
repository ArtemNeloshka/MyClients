using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Views;

namespace MyClients.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
	private readonly IUserService _userService;

	public RegisterViewModel(IUserService userService)
	{
		this._userService = userService;
	}
	
	[ObservableProperty]
	private string _name = String.Empty;
	[ObservableProperty]
	private string _namePlaceholder = "Enter your name...";
	[ObservableProperty]
	private Color _namePlaceholderColor = Colors.Black;
	
	[ObservableProperty]
	private string _surname = String.Empty;
	[ObservableProperty]
	private string _surnamePlaceholder = "Enter your surname...";
	[ObservableProperty]
	private Color _surnamePlaceholderColor = Colors.Black;
	
	[ObservableProperty]
	private string _email = String.Empty;
	[ObservableProperty]
	private string _emailPlaceholder = "Enter your email...";
	[ObservableProperty]
	private Color _emailPlaceholderColor = Colors.Black;
	
	[ObservableProperty] 
	private DateTime _birthdate = new DateTime(year: 2000, month: 1, day: 1);
	
	[ObservableProperty]
	private string _password = String.Empty;
	[ObservableProperty]
	private string _passwordPlaceholder = "Create a password";
	[ObservableProperty]
	private Color _passwordPlaceholderColor = Colors.Black;
	
	[ObservableProperty]
	private string _confirmPassword = String.Empty;
	[ObservableProperty]
	private string _confirmPasswordPlaceholder = "Enter your password again...";
	[ObservableProperty]
	private Color _confirmPasswordPlaceholderColor = Colors.Black;

	[RelayCommand]
	private async Task RegisterAsync()
	{
		// UI validation
		if (string.IsNullOrWhiteSpace(this.Name))
		{
			SetNameError(ErrorPlaceholders.NameIsEmpty);
			return;
		}
		if (this.Name.Length > ValidationRules.MaxNameLength)
		{
			SetNameError(ErrorPlaceholders.NameIsLong);
			return;
		}
		if (string.IsNullOrWhiteSpace(this.Surname))
		{
			SetSurnameError(ErrorPlaceholders.SurnameIsEmpty);
			return;
		}
		if (this.Surname.Length > ValidationRules.MaxSurnameLength)
		{
			SetSurnameError(ErrorPlaceholders.SurnameIsLong);
			return;
		}
		if (string.IsNullOrWhiteSpace(this.Email))
		{
			SetEmailError(ErrorPlaceholders.EmailIsEmpty);
			return;
		}
		if (!ValidationRules.IsValidEmail(this.Email))
		{
			SetEmailError(ErrorPlaceholders.InvalidEmail);
			return;
		}
		if (await _userService.GetUserByEmailAsync(this.Email) != null)
		{
			SetEmailError(ErrorPlaceholders.EmailAlreadyExists);
			return;
		}
		if (string.IsNullOrWhiteSpace(this.Password))
		{
			SetPasswordError(ErrorPlaceholders.RegistrationPasswordIsEmpty);
			return;
		}
		if (this.Password.Length < ValidationRules.MinPasswordLength)
		{
			SetPasswordError(ErrorPlaceholders.PasswordIsShort);
			return;
		}
		if (string.IsNullOrWhiteSpace(this.ConfirmPassword))
		{
			SetConfirmPasswordError(ErrorPlaceholders.ConfirmPasswordIsEmpty);
			return;
		}
		if (this.Password != this.ConfirmPassword)
		{
			SetConfirmPasswordError(ErrorPlaceholders.PasswordsDontMatch);
			return;
		}
		
		// BLL
		try
		{
			await _userService.RegisterUserAsync(this.Name, this.Surname, this.Email, 
				DateOnly.FromDateTime(this.Birthdate), this.Password);
			Session.CurrentUserEmail = this.Email;
			var newUser = await _userService.GetUserByEmailAsync(this.Email);

			if (newUser != null)
			{
				Session.CurrentUserId = newUser.Id;
			}

			Application.Current.MainPage = new AppShell();
		}
		catch (InvalidOperationException e)
		{
			Console.WriteLine(e.Message);
			SetEmailError(ErrorPlaceholders.EmailAlreadyExists);
		}
		catch (ArgumentException e)
		{
			Console.WriteLine(e.Message);
			await Shell.Current.DisplayAlertAsync("Увага!", "Перевірте правильність введених даних.", "Ok");
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			await Shell.Current.DisplayAlertAsync("Увага!", "Невідома помилка. Спробуй трохи пізніше!", "Ok");
		}
	}

	[RelayCommand]
	private void GoToLoginPage()
	{
		RedirectToLoginPage(null, false);
	}

	private void SetNameError(string placeholder)
	{
		this.Name = string.Empty;
		this.NamePlaceholder = placeholder;
		this.NamePlaceholderColor = Colors.Red;
	}
	
	private void SetSurnameError(string placeholder)
	{
		this.Surname = string.Empty;
		this.SurnamePlaceholder = placeholder;
		this.SurnamePlaceholderColor = Colors.Red;
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
	
	private void SetConfirmPasswordError(string placeholder)
	{
		this.ConfirmPassword = string.Empty;
		this.ConfirmPasswordPlaceholder = placeholder;
		this.ConfirmPasswordPlaceholderColor = Colors.Red;
	}

	partial void OnNameChanged(string value)
	{
		if (this.NamePlaceholderColor == Colors.Red)
		{
			this.NamePlaceholderColor = Colors.Black;
			this.NamePlaceholder = "Enter your name...";
		}
	}
	
	partial void OnSurnameChanged(string value)
	{
		if (this.SurnamePlaceholderColor == Colors.Red)
		{
			this.SurnamePlaceholderColor = Colors.Black;
			this.SurnamePlaceholder = "Enter your surname...";
		}
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
			this.PasswordPlaceholder = "Create your password";
		}
	}
	
	partial void OnConfirmPasswordChanged(string value)
	{
		if (this.ConfirmPasswordPlaceholderColor == Colors.Red)
		{
			this.ConfirmPasswordPlaceholderColor = Colors.Black;
			this.ConfirmPasswordPlaceholder = "Enter your password again...";
		}
	}
}
