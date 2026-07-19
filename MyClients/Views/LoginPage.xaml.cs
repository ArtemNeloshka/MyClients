using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.BLL.Interfaces;
using MyClients.Constants;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class LoginPage : ContentPage
{
	private LogInViewModel? _userViewModel;
	
	public LoginPage()
	{
		InitializeComponent();
	}
	
	protected override void OnAppearing()
	{
		base.OnAppearing();
		this._userViewModel = IPlatformApplication.Current.Services.GetService<LogInViewModel>();
	}

	private async void OnLoginClicked(object? sender, EventArgs e)
	{
		if (await VerifyLogInDataAsync())
			Application.Current.MainPage = new AppShell();
	}

	private async void OnRegisterClicked(object? sender, EventArgs e)
	{
		var page = Handler.MauiContext.Services.GetService<RegistrationPage>();
		await Navigation.PushAsync(page);
	}

	private async Task<bool> VerifyLogInDataAsync()
	{
		_userViewModel.Email = LoginEmailEntry.Text;
		_userViewModel.Password = LoginPasswordEntry.Text;
		
		var result = await _userViewModel.LogInUserAsync();

		if (result.ErrorMessage == null) return true;

		switch (result.ErrorMessage)
		{
			case ErrorPlaceholders.EmailIsEmpty:
				ChangeEntryInvalidInput(LoginEmailEntry, ErrorPlaceholders.EmailIsEmpty);
				break;
			
			case ErrorPlaceholders.LogInPasswordIsEmpty:
				ChangeEntryInvalidInput(LoginPasswordEntry, ErrorPlaceholders.LogInPasswordIsEmpty);
				break;
			
			case ErrorPlaceholders.InvalidEmail:
				ChangeEntryInvalidInput(LoginEmailEntry, ErrorPlaceholders.InvalidEmail);
				break;
			
			case ErrorPlaceholders.LogInEmailNotFound:
				ChangeEntryInvalidInput(LoginEmailEntry, ErrorPlaceholders.LogInEmailNotFound);
				break;
			
			case ErrorPlaceholders.PasswordIsShort:
				ChangeEntryInvalidInput(LoginPasswordEntry, ErrorPlaceholders.PasswordIsShort);
				break;
			
			case ErrorPlaceholders.PasswordIncorrect:
				ChangeEntryInvalidInput(LoginPasswordEntry, ErrorPlaceholders.PasswordIncorrect);
				break;
			
			default:
				ChangeEntryInvalidInput(LoginEmailEntry, result.ErrorMessage);
				break;
		}
		
		return false;
	}
	
	private static void ChangeEntryInvalidInput(Entry entry, string placeholder)
	{
		entry.Text = string.Empty;
		entry.Placeholder = placeholder;
		entry.PlaceholderColor = Colors.Red;
	}
}