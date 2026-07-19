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