using MyClients.ViewModels;

namespace MyClients.Views;

public partial class DisciplinesPage : ContentPage
{
	private readonly DisciplinesViewModel? _viewModel;
	public DisciplinesPage()
	{
		InitializeComponent();
		_viewModel = IPlatformApplication.Current?.Services.GetService<DisciplinesViewModel>();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_viewModel != null)
		{
			await _viewModel.LoadDisciplinesAsync();
		}
	}
}
