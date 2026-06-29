using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClients.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

	private async void OnDisciplinePickerTapped(object? sender, EventArgs e)
	{
		var result = await DisplayActionSheetAsync(
			"Select discipline", "Cancel", null,
			"Bouldering", "Top rope", "Lead", "Speed");

		if (result != null && result != "Cancel")
			DisciplineLabel.Text = result;
	}

	private async void OnGoToLoginPageClicked(object? sender, EventArgs e)
	{
		var loginPage = IPlatformApplication.Current.Services.GetService<LoginPage>();
		Application.Current.MainPage = new NavigationPage(loginPage);
	}
}