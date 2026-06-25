using MyClients.BLL.Interfaces;

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
			return (false, "Name cannot be empty.");
		}
		if (string.IsNullOrWhiteSpace(Surname))
		{
			return (false, "Surname cannot be empty.");
		}
		if (string.IsNullOrWhiteSpace(Email))
		{
			return (false, "Email cannot be empty.");
		}
		if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
		{
			return (false, "Password cannot be less than 8 symbols.");
		}
		if (string.IsNullOrWhiteSpace(ConfirmPassword))
		{
			return (false, "Password confirmation cannot be empty.");
		}
		if (Password != ConfirmPassword)
		{
			return (false, "Passwords don't match.");
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