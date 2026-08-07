using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;

namespace MyClients.ViewModels;

[QueryProperty(nameof(TrainingId), "TrainingId")]
public partial class TrainingDetailsViewModel : BaseViewModel
{
	[ObservableProperty]
	private int _trainingId;
	
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

	async partial void OnTrainingIdChanged(int value)
	{
		try
		{
			await LoadTrainingAsync(value);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Cannot find your training. Try later!", isError: true);
		}
	}

	[ObservableProperty]
	private string _name = string.Empty;
	[ObservableProperty]
	private string _surname = string.Empty;

	[ObservableProperty]
	private DateOnly _trainingDate;

	[ObservableProperty]
	private string _trainingLog;

	[ObservableProperty]
	private string _boulderingBestGrade;

	[ObservableProperty]
	private string _topRopeBestGrade;

	[ObservableProperty]
	private string _leadBestGrade;

	[ObservableProperty]
	private string _speedBestGrade = "-";

	private async Task LoadTrainingAsync(int trainingId)
	{
		var userEmail = Session.CurrentUserEmail;
		if (string.IsNullOrEmpty(userEmail))
		{
			Console.WriteLine($"Email is empty.");
			RedirectToLoginPage("Couldn't find your training. Try later!");
			return;
		}

		try
		{
			var user = await _userService.GetUserByEmailAsync(userEmail);

			if (user == null)
			{
				Console.WriteLine($"User for email {userEmail} doesn't have an account");
				RedirectToLoginPage("Couldn't find your account. Try later!");
				return;
			}

			Name = user.Name;
			Surname = user.Surname;

			var training = await _trainingService.GetTrainingByIdAsync(trainingId);

			if (training == null)
			{
				Console.WriteLine($"Couldn't find a training with id={trainingId}");
				await GoBackWithAlertAsync(
					message: "We cannot find your training. Try later!",
					isError: true);
				return;
			}

			// TODO: GetBestGradeByTrainingId method
			BoulderingBestGrade = "N/A";
			TopRopeBestGrade = "N/A";
			LeadBestGrade = "N/A";
			SpeedBestGrade = "N/A";

			TrainingDate = training.TrainingDate;
			TrainingLog = training.TrainingLog;
		}
		catch (ArgumentException e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Cannot find your training. Try later!", isError: true);
		}
		catch (KeyNotFoundException e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Some record isn't found in DB", isError: true);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			await GoBackWithAlertAsync("Somthn weird happened", isError: true);
		}
	}
}
