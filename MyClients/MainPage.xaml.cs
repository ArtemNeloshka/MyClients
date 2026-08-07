using MyClients.ViewModels;

namespace MyClients;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel? _viewModel;
	
	public MainPage(MainViewModel viewModel)
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
			await _viewModel.LoadUserNameAsync();
		}
	}
}
