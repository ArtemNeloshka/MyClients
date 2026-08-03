using MyClients.ViewModels;

namespace MyClients;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel? _viewModel;
	
	public MainPage()
	{
		InitializeComponent();
		_viewModel = IPlatformApplication.Current?.Services.GetService<MainViewModel>();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_viewModel != null)
		{
			await _viewModel.LoadUserNameAsync();
		}
	}
}
