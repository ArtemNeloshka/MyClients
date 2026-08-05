using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Models;
using MyClients.Views;

namespace MyClients.ViewModels;

[QueryProperty(nameof(AlertMessage), Navigation.AlertMessageKey)]
[QueryProperty(nameof(IsErrorAlert), Navigation.IsErrorKey)]
public partial class WorkoutsArchiveViewModel : BaseViewModel
{
	private readonly ITrainingService _trainingService;

	public WorkoutsArchiveViewModel(ITrainingService trainingService)
	{
		this._trainingService = trainingService;
	}

	private string _alertMessage;

	public string AlertMessage
	{
		get => _alertMessage;
		set
		{
			_alertMessage = value;
			if (string.IsNullOrEmpty(value))
			{
				Shell.Current.DisplayAlertAsync(
					IsErrorAlert ? "Error" : "Success",
					value,
					"Ok");

				_alertMessage = string.Empty;
			}
		}
	}
	
	public bool IsErrorAlert { get; set; }
	
	[ObservableProperty]
	private ObservableCollection<TrainingCardModel> _trainingCards = [];

	public async Task LoadTrainingsAsync()
	{
		var userId = Session.CurrentUserId;
		if (userId == null)
		{
			await GoBackWithAlertAsync("We cannot find your account. Try later!", isError: true);
			return;
		}

		try
		{
			var trainingsByUser = await _trainingService.GetTrainingsByUserIdAsync((int)userId);

			TrainingCards.Clear();

			foreach (var training in trainingsByUser)
			{
				var duration = training.TrainingDuration;
				var durationToString = $"{duration.Hours}hr {duration.Minutes}m";
				var bestGradesClimbed = await _trainingService.GetTopAttemptsByTrainingIdAsync(training.Id, 3);
				var bestGradesClimbedToStringArray = bestGradesClimbed
					.Select(a => $"{a.Grade.Name} ({a.Discipline.Name})")
					.ToArray();
				var bestGradesClimbedText = string.Join("\n", bestGradesClimbedToStringArray);
				var disciplinesTrained = await _trainingService.GetAllDisciplinesByTrainingIdAsync(training.Id);
				var disciplinesTrainedToStringArray = disciplinesTrained
					.Select(d => d.Name)
					.ToArray();
				var disciplinedTrainedText = string.Join("\n", disciplinesTrainedToStringArray);

				TrainingCards.Add(new TrainingCardModel
				{
					TrainingId = training.Id,
					TrainingDate = training.TrainingDate.ToString("dd.MM.yyyy"),
					TrainingDuration = durationToString,
					DisciplinesTrained = disciplinedTrainedText,
					BestGradesClimbed = bestGradesClimbedText,
					ClimbingGym = "Climbing Gym",
				});
			}
		}
		catch (ArgumentException e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Couldn't find your trainings. Try later!", isError: true);
		}
		catch (KeyNotFoundException e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Couldn't find your trainings. Try later!", isError: true);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Couldn't find your trainings for some reason. Try later!", isError: true);
		}
	}

	[RelayCommand]
	private async Task GoToDetailsAsync(int trainingId)
	{
		var navigationParameter = new Dictionary<string, object>
		{
			{"TrainingId", trainingId}
		};

		await Shell.Current.GoToAsync(nameof(TrainingDetailsPage), navigationParameter);
	}
}
