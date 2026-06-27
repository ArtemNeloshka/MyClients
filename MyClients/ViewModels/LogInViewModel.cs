using MyClients.BLL.Interfaces;

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
			return (false, "Please, enter your email");
		}

		if (string.IsNullOrWhiteSpace(Password))
		{
			return (false, "Please, enter your password");
		}

		try
		{
			var result = await _userService.LoginUserAsync(Email, Password);
			if (result)
				return (true, null);

			return (false, "Incorrect password");
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return (false, e.Message);
		}
	}
}