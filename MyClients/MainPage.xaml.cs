using MyClients.BLL.Interfaces;

namespace MyClients;

public partial class MainPage : ContentPage
{
	private IUserService? _userService;

	public MainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		this._userService = IPlatformApplication.Current.Services.GetService<IUserService>();
	}
}