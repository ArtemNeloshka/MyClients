using MyClients.ViewModels;

namespace MyClients.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegisterViewModel registerViewModel)
	{
		InitializeComponent();
		BindingContext = registerViewModel;
	}
}
