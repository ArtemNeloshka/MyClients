using MyClients.DAL;
using MyClients.Views;

namespace MyClients;

public partial class App : Application
{
	private readonly LoginPage _loginPage;

	public App(LoginPage loginPage, MyClientsDbContext dbContext)
	{
		InitializeComponent();
		_loginPage = loginPage;

		Task.Run(() => dbContext.Database.EnsureCreated());
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(_loginPage));
	}
}