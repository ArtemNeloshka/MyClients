namespace MyClients.Domain.Constants;

public static class ErrorPlaceholders
{
	public const string NameIsEmpty = "Enter your name, please";
	public const string NameIsLong = "Name is too long. Please, enter up to 20 symbols.";
	public const string SurnameIsEmpty = "Enter you surname, please";
	public const string SurnameIsLong = "Surname is too long. Please, enter up to 20 symbols.";
	public const string EmailIsEmpty = "Enter you email, please";
	public const string LogInEmailNotFound = "This email address doesn't have an account. Try to register";
	public const string RegistrationPasswordIsEmpty = "Create a password";
	public const string LogInPasswordIsEmpty = "Please, enter your password";
	public const string PasswordIsShort = "Password is too short, at least 8 symbols required";
	public const string ConfirmPasswordIsEmpty = "Confirm your password, please";
	public const string PasswordsDontMatch = "Passwords don't match, try again";
	public const string PasswordIncorrect = "Incorrect password. Try again";
	public const string InvalidEmail = "Please, enter your @gmail.com email";
	public const string EmailAlreadyExists = "User with this email already has an account";
}