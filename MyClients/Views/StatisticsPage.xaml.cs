using MyClients.ViewModels;

namespace MyClients.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
