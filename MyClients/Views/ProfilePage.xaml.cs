using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = IPlatformApplication.Current.Services.GetService<ProfileViewModel>();
		BindingContext = viewModel;
		await viewModel.LoadUserAsync();
	}

	private async void OnDisciplinePickerTapped(object? sender, EventArgs e)
	{
		var viewModel = (ProfileViewModel)BindingContext;
		var names = viewModel.DisciplineNames.ToArray();
		
		var result = await DisplayActionSheetAsync(
			"Select discipline", "Cancel", null, names);

		if (result != null && result != "Cancel")
			DisciplineLabel.Text = result;
	}

	private async void OnGoToLoginPageClicked(object? sender, EventArgs e)
	{
		var loginPage = IPlatformApplication.Current.Services.GetService<LoginPage>();
		Application.Current.MainPage = new NavigationPage(loginPage);
	}
}
