using System.Collections.ObjectModel;
using System.ComponentModel;
using MyClients.BLL.Interfaces;
using MyClients.Constants;
using MyClients.DAL.Entities;
using System.Linq;

namespace MyClients.ViewModels;

public class TrainingDetailsViewModel : INotifyPropertyChanged
{
	private readonly IUserService _userService;
	private readonly IDisciplineService _disciplineService;
	private readonly ITrainingService _trainingService;

	public TrainingDetailsViewModel(IUserService userService, IDisciplineService disciplineService,
		ITrainingService trainingService)
	{
		this._userService = userService;
		this._disciplineService = disciplineService;
		this._trainingService = trainingService;
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

	private DateOnly _trainingDate;
	public DateOnly TrainingDate
	{
		get => _trainingDate;
		set
		{
			_trainingDate = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrainingDate)));
		}
	}

	private string _trainingLog;
	public string TrainingLog
	{
		get => _trainingLog;
		set
		{
			_trainingLog = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrainingLog)));
		}
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

	public async Task LoadTrainingAsync(int trainingId)
	{
		// if (string.IsNullOrEmpty(Session.CurrentUserEmail))
		// 	throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		var user = await _userService.GetUserByEmailAsync("test@gmail.com");

		if (user == null)
			throw new KeyNotFoundException(ErrorMessages.UserNotFound);

		Name = user.Name;
		Surname = user.Surname;

		var training = await _trainingService.GetTrainingByIdAsync(trainingId);
		
		BoulderingBestGrade = "N/A";
		TopRopeBestGrade = "N/A";
		LeadBestGrade = "N/A";
		SpeedBestGrade = "N/A";

		TrainingDate = training.TrainingDate;
		TrainingLog = training.TrainingLog;
		TrainingLog += "wqef ewfjoewj coew joj cowj coewjcow owm oew mcow cwo";
	}
}