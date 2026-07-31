using MyClients.BLL.Interfaces;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

namespace MyClients.ViewModels;

public class RegisterViewModel
{
	private readonly IUserService _userService;

	public RegisterViewModel(IUserService userService)
	{
		this._userService = userService;
	}
	
	public string Name { get; set; } = String.Empty;
	public string Surname { get; set; } = String.Empty;
	public string Email { get; set; } = String.Empty;
	public DateOnly Birthdate { get; set; }
	public string Password { get; set; } = String.Empty;
	public string ConfirmPassword { get; set; } = String.Empty;

	public async Task<(bool Success, string? ErrorMessage)> RegisterAsync()
	{
		// UI validation
		if (string.IsNullOrWhiteSpace(Name))
		{
			return (false, ErrorPlaceholders.NameIsEmpty);
		}
		if (Name.Length > ValidationRules.MaxNameLength)
		{
			return (false, ErrorPlaceholders.NameIsLong);
		}
		if (string.IsNullOrWhiteSpace(Surname))
		{
			return (false, ErrorPlaceholders.SurnameIsEmpty);
		}
		if (Surname.Length > ValidationRules.MaxSurnameLength)
		{
			return (false, ErrorPlaceholders.SurnameIsLong);
		}
		if (string.IsNullOrWhiteSpace(Email))
		{
			return (false, ErrorPlaceholders.EmailIsEmpty);
		}
		if (!ValidationRules.IsValidEmail(Email))
		{
			return (false, ErrorPlaceholders.InvalidEmail);
		}
		if (await _userService.GetUserByEmailAsync(Email) != null)
		{
			return (false, ErrorPlaceholders.EmailAlreadyExists);
		}
		if (string.IsNullOrWhiteSpace(Password))
		{
			return (false, ErrorPlaceholders.RegistrationPasswordIsEmpty);
		}
		if (Password.Length < ValidationRules.MinPasswordLength)
		{
			return (false, ErrorPlaceholders.PasswordIsShort);
		}
		if (string.IsNullOrWhiteSpace(ConfirmPassword))
		{
			return (false, ErrorPlaceholders.ConfirmPasswordIsEmpty);
		}
		if (Password != ConfirmPassword)
		{
			return (false, ErrorPlaceholders.PasswordsDontMatch);
		}
		
		// BLL
		try
		{
			await _userService.RegisterUserAsync(Name, Surname, Email, Birthdate, Password);
			return (true, null);
		}
		catch (Exception e)
		{
			return (false, e.Message);
		}
	}
}