using MyClients.ViewModels;

namespace MyClients.Views;

public partial class TrainingDetailsPage : ContentPage
{
	public TrainingDetailsPage(TrainingDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
	}
}
