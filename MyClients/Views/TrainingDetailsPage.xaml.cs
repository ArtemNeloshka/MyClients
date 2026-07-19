using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class TrainingDetailsPage : ContentPage
{
	public int TrainingId { get; set; }
	public TrainingDetailsPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = IPlatformApplication.Current.Services.GetService<TrainingDetailsViewModel>();
		BindingContext = viewModel;
		await viewModel.LoadTrainingAsync(TrainingId);
	}
}