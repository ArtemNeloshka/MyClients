namespace MyClients.Domain.Constants;

public static class ErrorMessages
{
	public const string InvalidDateHigherThanToday = "Date cannot be higher than today";
	public const string UserNotFound = "User is not found.";
	public const string InvalidEmail = "Email is not valid.";
	public const string PasswordIncorrect = "Password doesn't match to existed";
	public const string NameIsNullOrEmpty = "Name cannot be null.";
	public const string SurnameIsNullOrEmpty = "Surname cannot be null.";
	public const string PasswordIsShort = "Password cannot be less then 8 symbols.";
	public const string EmailAlreadyExists = "User with this email already exists.";
	public const string PersonalRecordNotFound = "Personal record is not found.";
	public const string TrainingLogIsNullOrEmpty = "Log cannot be empty.";
	public const string TrainingNotFound = "Training is not found.";
	public const string DateStartHigherThanEnd = "Start date cannot be higher than an end date.";
	public const string DisciplineNotFoundMessage = "Discipline is not found.";
}