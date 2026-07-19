using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClients.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
	}

	private async void OnOpenWorkoutsArchivePageClicked(object? sender, EventArgs e)
	{
		var workoutsArchivePage = Handler.MauiContext.Services.GetService<WorkoutsArchivePage>();
		await Navigation.PushAsync(workoutsArchivePage);
	}
}