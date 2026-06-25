using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClients.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}

	private async void OnBackToLogInPageClicked(object? sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	private void OnRegisterClicked(object? sender, EventArgs e)
	{
		throw new NotImplementedException();
	}
}