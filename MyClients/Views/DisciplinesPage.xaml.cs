using MyClients.ViewModels;

namespace MyClients.Views;

public partial class DisciplinesPage : ContentPage
{
	private readonly DisciplinesViewModel? _viewModel;
	public DisciplinesPage(DisciplinesViewModel viewModel)
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
			await _viewModel.LoadDisciplinesAsync();
		}
	}
}
