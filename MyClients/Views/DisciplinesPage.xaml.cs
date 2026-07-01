using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClients.BLL.Services;
using MyClients.DAL.Entities;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class DisciplinesPage : ContentPage
{
	public DisciplinesPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = IPlatformApplication.Current.Services.GetService<DisciplinesViewModel>();
		BindingContext = viewModel;
		await viewModel.LoadDisciplinesAsync();
	}
}