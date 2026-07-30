using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyClients.BLL.Interfaces;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.Domain.Entities;

namespace MyClients.ViewModels;

public class WorkoutsArchiveViewModel : INotifyPropertyChanged
{
	private readonly ITrainingService _trainingService;

	public WorkoutsArchiveViewModel(ITrainingService trainingService)
	{
		this._trainingService = trainingService;
	}
	
	public event PropertyChangedEventHandler? PropertyChanged;
	
	private ObservableCollection<Training> _trainings = new ObservableCollection<Training>();
	public ObservableCollection<Training> Trainings
	{
		get => _trainings;
		set
		{
			_trainings = value;
			OnPropertyChanged(nameof(Trainings));
		}
	}

	public async Task LoadTrainingsAsync()
	{
		var trainingsByUser = await _trainingService.GetTrainingsByUserIdAsync(1);

		foreach (var training in trainingsByUser)
		{
			Trainings.Add(training);
		}
	}
	
	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}