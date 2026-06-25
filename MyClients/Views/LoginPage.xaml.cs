using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.BLL.Interfaces;

namespace MyClients.Views;

public partial class LoginPage : ContentPage
{
	private readonly IUserService _userService;
	
	public LoginPage(IUserService userService)
	{
		InitializeComponent();
		this._userService = userService;
	}

	private void OnLoginClicked(object? sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	private async void OnRegisterClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegistrationPage());
	}
}