using MyClients.ViewModels;

namespace MyClients.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LogInViewModel logInViewModel)
	{
		InitializeComponent();
		BindingContext = logInViewModel;
	}
}
