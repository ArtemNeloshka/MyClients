using System.ComponentModel;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

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

	private string _boulderingBestGrade = "-";
	public string BoulderingBestGrade
	{
		get => _boulderingBestGrade;
		set
		{
			_boulderingBestGrade = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoulderingBestGrade)));
		}
	}

	private string _topRopeBestGrade = "-";
	public string TopRopeBestGrade
	{
		get => _topRopeBestGrade;
		set
		{
			_topRopeBestGrade = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopRopeBestGrade)));
		}
	}

	private string _leadBestGrade = "-";
	public string LeadBestGrade
	{
		get => _leadBestGrade;
		set
		{
			_leadBestGrade = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeadBestGrade)));
		}
	}

	private string _speedBestGrade = "-";
	public string SpeedBestGrade
	{
		get => _speedBestGrade;
		set
		{
			_speedBestGrade = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeedBestGrade)));
		}
	}

	public List<string> DisciplineNames { get; set; } = new();

	public async Task LoadUserAsync()
	{
		var userEmail = Session.CurrentUserEmail;
		if (string.IsNullOrEmpty(userEmail))
			throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		var user = await _userService.GetUserByEmailAsync(userEmail);
		
		if (user == null)
			throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		Name = user.Name;
		Surname = user.Surname;
		Email = user.Email;
		Birthdate = user.Birthday;

		var disciplines = await _disciplineService.GetAllDisciplinesAsync();
		DisciplineNames = disciplines.Select(d => d.Name).ToList();

		var bouldering = await _disciplineService.GetDisciplineByNameAsync(Disciplines.Bouldering);
		BoulderingBestGrade = (await _userService
			.GetBestGradeInDisciplineAsync(user.Id, bouldering.Id))?.Name ?? "-";
		
		var topRope = await _disciplineService.GetDisciplineByNameAsync(Disciplines.TopRopeClimbing);
		TopRopeBestGrade = (await _userService
				.GetBestGradeInDisciplineAsync(user.Id, topRope.Id))?.Name ?? "-";
		
		var lead = await _disciplineService.GetDisciplineByNameAsync(Disciplines.LeadClimbing);
		LeadBestGrade = (await _userService
				.GetBestGradeInDisciplineAsync(user.Id, lead.Id))?.Name ?? "-";
		
		var speed = await _disciplineService.GetDisciplineByNameAsync(Disciplines.SpeedClimbing);
		SpeedBestGrade = (await _userService
				.GetBestGradeInDisciplineAsync(user.Id, speed.Id))?.Name ?? "-";
	}
}