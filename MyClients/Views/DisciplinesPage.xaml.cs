using MyClients.ViewModels;

namespace MyClients.Views;

public partial class DisciplinesPage : ContentPage
{
	private readonly DisciplinesViewModel _viewModel;
	public DisciplinesPage(DisciplinesViewModel viewModel)
	{
		InitializeComponent();
		this._viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadDisciplinesAsync();
	}
}
