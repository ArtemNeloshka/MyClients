using System.ComponentModel;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

namespace MyClients.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
	private readonly IUserService _userService;

	public MainViewModel(IUserService userService)
	{
		this._userService = userService;
	}
	
	public event PropertyChangedEventHandler? PropertyChanged;
	
	private string _userName = String.Empty;

	public string UserName
	{
		get => _userName;
		set
		{
			_userName = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
		}
	}

	public async Task LoadUserNameAsync()
	{
		var user = await _userService.GetUserByEmailAsync(Session.CurrentUserEmail);
		UserName = user?.Name ?? string.Empty;
	}
}