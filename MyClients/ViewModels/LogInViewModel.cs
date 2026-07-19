using MyClients.BLL.Interfaces;
using MyClients.Constants;

namespace MyClients.ViewModels;

public class LogInViewModel
{
	private readonly IUserService _userService;

	public LogInViewModel(IUserService userService)
	{
		this._userService = userService;
	}

	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;

	public async Task<(bool Success, string? ErrorMessage)> LogInUserAsync()
	{
		if (string.IsNullOrWhiteSpace(Email))
		{
			return (false, ErrorPlaceholders.EmailIsEmpty);
		}

		if (!ValidationRules.IsValidEmail(Email))
		{
			return (false, ErrorPlaceholders.InvalidEmail);
		}

		if (string.IsNullOrWhiteSpace(Password))
		{
			return (false, ErrorPlaceholders.LogInPasswordIsEmpty);
		}

		if (Password.Length < ValidationRules.MinPasswordLength)
		{
			return (false, ErrorPlaceholders.PasswordIsShort);
		}

		try
		{
			var result = await _userService.LoginUserAsync(Email, Password);
			if (result.Success)
			{
				Session.CurrentUserEmail = Email;
				return (true, null);
			}

			if (result.ErrorMessage == ErrorMessages.UserNotFound)
				return (false, ErrorPlaceholders.LogInEmailNotFound);

			if (result.ErrorMessage == ErrorMessages.PasswordIncorrect)
				return (false, ErrorPlaceholders.PasswordIncorrect);

			return (false, result.ErrorMessage);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return (false, e.Message);
		}
	}
}