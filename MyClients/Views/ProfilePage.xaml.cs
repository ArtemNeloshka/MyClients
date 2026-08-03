using MyClients.ViewModels;

namespace MyClients.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel? _viewModel;
	public ProfilePage()
	{
		InitializeComponent();
		_viewModel = IPlatformApplication.Current?.Services.GetService<ProfileViewModel>();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_viewModel != null)
		{
			await _viewModel.LoadUserAsync();
		}
	}
}
