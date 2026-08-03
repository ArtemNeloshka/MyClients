using MyClients.ViewModels;

namespace MyClients.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
		BindingContext = IPlatformApplication.Current?.Services.GetService<StatisticsPageViewModel>();
	}
}
