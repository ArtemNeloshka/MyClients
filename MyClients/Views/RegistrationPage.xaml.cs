using MyClients.Domain.Constants;
using MyClients.ViewModels;
using MyClients.Views.Controls;

namespace MyClients.Views;

public partial class RegistrationPage : ContentPage
{
	private RegisterViewModel? _registerViewModel;
	
	public RegistrationPage()
	{
		InitializeComponent();
	}
	
	protected override void OnAppearing()
	{
		base.OnAppearing();
		this._registerViewModel = IPlatformApplication.Current.Services.GetService<RegisterViewModel>();
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
				case ErrorPlaceholders.NameIsEmpty:
					ChangeEntryInvalidInput(EntryNameRegistration, ErrorPlaceholders.NameIsEmpty);
					break;
				
				case ErrorPlaceholders.NameIsLong:
					ChangeEntryInvalidInput(EntryNameRegistration, ErrorPlaceholders.NameIsLong);
					break;
					
				case ErrorPlaceholders.SurnameIsEmpty:
					ChangeEntryInvalidInput(EntrySurnameRegistration, ErrorPlaceholders.SurnameIsEmpty);
					break;
				
				case ErrorPlaceholders.SurnameIsLong:
					ChangeEntryInvalidInput(EntrySurnameRegistration, ErrorPlaceholders.SurnameIsLong);
					break;
					
				case ErrorPlaceholders.EmailIsEmpty:
					ChangeEntryInvalidInput(EntryEmailRegistration, ErrorPlaceholders.EmailIsEmpty);
					break;
					
				case ErrorPlaceholders.RegistrationPasswordIsEmpty:
					ChangeEntryInvalidInput(EntryPasswordRegistration, ErrorPlaceholders.RegistrationPasswordIsEmpty);
					break;
				
				case ErrorPlaceholders.PasswordIsShort:
					ChangeEntryInvalidInput(EntryPasswordRegistration, ErrorPlaceholders.PasswordIsShort);
					
					EntryConfirmPasswordRegistration.Text = string.Empty;
					EntryConfirmPasswordRegistration.Placeholder = LogInPagePlaceholders.ConfirmPassword;
					break;
					
				case ErrorPlaceholders.ConfirmPasswordIsEmpty:
					ChangeEntryInvalidInput(EntryConfirmPasswordRegistration, ErrorPlaceholders.ConfirmPasswordIsEmpty);
					break;
					
				case ErrorPlaceholders.PasswordsDontMatch:
					ChangeEntryInvalidInput(EntryConfirmPasswordRegistration, ErrorPlaceholders.PasswordsDontMatch);
					break;
					
				case ErrorPlaceholders.InvalidEmail:
					ChangeEntryInvalidInput(EntryEmailRegistration, ErrorPlaceholders.InvalidEmail);
					break;
				
				case ErrorPlaceholders.EmailAlreadyExists:
					ChangeEntryInvalidInput(EntryEmailRegistration, ErrorPlaceholders.EmailAlreadyExists);
					break;
				
				case null:
					ChangeEntryInvalidInput(EntryNameRegistration, "Something weird happend");
					break;
				
				default:
					ChangeEntryInvalidInput(EntryConfirmPasswordRegistration, registerResult.ErrorMessage);
					break;
			}
		}
	}

	private static void ChangeEntryInvalidInput(RegistrationTextFieldView entry, string placeholder)
	{
		entry.Text = string.Empty;
		entry.Placeholder = placeholder;
		entry.PlaceholderColor = Colors.Red;
	}
}