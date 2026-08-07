using MyClients.ViewModels;

namespace MyClients.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel? _viewModel;
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
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
