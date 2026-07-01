using System.Collections.ObjectModel;
using System.ComponentModel;
using MyClients.BLL.Interfaces;
using MyClients.Constants;

namespace MyClients.ViewModels;

public class ProfileViewModel : INotifyPropertyChanged
{
	private readonly IUserService _userService;
	private readonly IDisciplineService _disciplineService;

	public ProfileViewModel(IUserService userService, IDisciplineService disciplineService)
	{
		this._userService = userService;
		this._disciplineService = disciplineService;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private string _name = string.Empty;
	public string Name
	{
		get => _name;
		set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); }
	}

	private string _surname = string.Empty;
	public string Surname
	{
		get => _surname;
		set { _surname = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Surname))); }
	}

	private string _email = string.Empty;
	public string Email
	{
		get => _email;
		set { _email = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email))); }
	}

	private DateOnly _birthdate;
	public DateOnly Birthdate
	{
		get => _birthdate;
		set { _birthdate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Birthdate))); }
	}

	public List<string> DisciplineNames { get; set; } = new();

	public async Task LoadUserAsync()
	{
		// if (string.IsNullOrEmpty(Session.CurrentUserEmail))
		// 	throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		var user = await _userService.GetUserByEmailAsync("artem@gmail.com");

		if (user == null)
			throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		Name = user.Name;
		Surname = user.Surname;
		Email = user.Email;
		Birthdate = user.Birthday;

		var disciplines = await _disciplineService.GetAllDisciplinesAsync();
		DisciplineNames = disciplines.Select(d => d.Name).ToList();
	}
}