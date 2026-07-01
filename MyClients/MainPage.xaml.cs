using MyClients.BLL.Interfaces;
using MyClients.ViewModels;

namespace MyClients;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = IPlatformApplication.Current.Services.GetService<MainViewModel>();
		BindingContext = viewModel;
		await viewModel.LoadUserNameAsync();
	}
}