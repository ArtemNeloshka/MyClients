using Microsoft.Maui.Controls.Shapes;
using MyClients.Domain.Entities;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class WorkoutsArchivePage : ContentPage
{
	public WorkoutsArchivePage(WorkoutsArchiveViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is WorkoutsArchiveViewModel viewModel)
		{
			await viewModel.LoadTrainingsAsync();
		}
	}
}
