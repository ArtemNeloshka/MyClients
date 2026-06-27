using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.BLL.Interfaces;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class LoginPage : ContentPage
{
	private readonly LogInViewModel _userViewModel;
	
	public LoginPage(LogInViewModel userViewModel)
	{
		InitializeComponent();
		this._userViewModel = userViewModel;
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
			case "Please, enter your email":
				ChangeEntryInvalidInput(LoginEmailEntry, result.ErrorMessage);
				break;
			
			case "Please, enter your password":
				ChangeEntryInvalidInput(LoginPasswordEntry, result.ErrorMessage);
				break;
			
			case "Email is not valid.":
				ChangeEntryInvalidInput(LoginEmailEntry, "Please, enter your @gmail.com email");
				break;
			
			case "Password cannot be shorter then 8 symbols.":
				ChangeEntryInvalidInput(LoginPasswordEntry, result.ErrorMessage);
				break;
			
			case "Incorrect password":
				ChangeEntryInvalidInput(LoginPasswordEntry, "Incorrect password. Try again");
				break;
			
			default:
				
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