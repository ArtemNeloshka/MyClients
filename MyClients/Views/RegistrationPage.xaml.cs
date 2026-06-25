using MyClients.ViewModels;

namespace MyClients.Views;

public partial class RegistrationPage : ContentPage
{
	private readonly RegisterViewModel _registerViewModel;
	
	public RegistrationPage(RegisterViewModel registerViewModel)
	{
		InitializeComponent();
		this._registerViewModel = registerViewModel;
	}

	private async void OnBackToLogInPageClicked(object? sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	private async void OnRegisterClicked(object? sender, EventArgs e)
	{
		_registerViewModel.Name = EntryNameRegistration.Text;
		_registerViewModel.Surname = EntrySurnameRegistration.Text;
		_registerViewModel.Email = EntryEmailRegistration.Text;
		_registerViewModel.Birthdate = DateOnly.FromDateTime((DateTime)RegistrationBirthDatePicker.Date);
		_registerViewModel.Password = EntryPasswordRegistration.Text;
		_registerViewModel.ConfirmPassword = EntryConfirmPasswordRegistration.Text;
		
		var registerResult = await _registerViewModel.RegisterAsync();

		if (registerResult.Success)
		{
			await Navigation.PopAsync();
		}
		else
		{
			switch (registerResult.ErrorMessage)
			{
				case "Name cannot be empty.":
					ChangeEntryInvalidInput(EntryNameRegistration, "Enter your name, please");
					break;
					
				case "Surname cannot be empty.":
					ChangeEntryInvalidInput(EntrySurnameRegistration, "Enter you surname, please");
					break;
					
				case "Email cannot be empty.":
					ChangeEntryInvalidInput(EntryEmailRegistration, "Enter you email, please");
					break;
					
				case "Password cannot be less than 8 symbols.":
					ChangeEntryInvalidInput(EntryPasswordRegistration, 
						"Password is too short, at least 8 symbols required");
					
					EntryConfirmPasswordRegistration.Text = string.Empty;
					EntryConfirmPasswordRegistration.Placeholder = "Enter your password again...";
					break;
					
				case "Password confirmation cannot be empty.":
					ChangeEntryInvalidInput(EntryConfirmPasswordRegistration, "Confirm your password, please");
					break;
					
				case "Passwords don't match.":
					ChangeEntryInvalidInput(EntryPasswordRegistration, "Passwords don't match, try again");

					EntryConfirmPasswordRegistration.Text = string.Empty;
					EntryConfirmPasswordRegistration.Placeholder = "Enter your password again...";
					break;
					
				case "Email doesn't match the pattern.":
					ChangeEntryInvalidInput(EntryEmailRegistration, "Please, enter your @gmail.com email");
					break;
				
				case "User with this email already exists.":
					ChangeEntryInvalidInput(EntryEmailRegistration, "User with this email already has an account");
					break;
				
				default:
					ChangeEntryInvalidInput(EntryConfirmPasswordRegistration, registerResult.ErrorMessage);
					break;
			}
		}
	}

	private static void ChangeEntryInvalidInput(Entry entry, string placeholder)
	{
		entry.Text = string.Empty;
		entry.Placeholder = placeholder;
		entry.PlaceholderColor = Colors.Red;
	}
}