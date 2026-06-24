using MyClients.BLL.Interfaces;

namespace MyClients;

public partial class MainPage : ContentPage
{
	private readonly IUserService _userService;

	public MainPage(IUserService userService)
	{
		InitializeComponent();
		_userService = userService;
	}
}