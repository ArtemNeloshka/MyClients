using System.Text.RegularExpressions;

namespace MyClients.Domain.Constants;

public static class ValidationRules
{
	public const int MinNameLength = 0;
	public const int MaxNameLength = 20;
	public const int MinSurnameLength = 0;
	public const int MaxSurnameLength = 20;
	public const int MinPasswordLength = 8;
	public const int MaxTrainingLogLength = 1000;
	
	private const string EmailPattern = @"^[\w\.-]+@gmail\.com$";
    
	public static bool IsValidEmail(string email) 
		=> Regex.IsMatch(email, EmailPattern);
}