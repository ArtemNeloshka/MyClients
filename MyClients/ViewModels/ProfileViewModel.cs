using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Views;

namespace MyClients.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
	private readonly IUserService _userService;
	private readonly IDisciplineService _disciplineService;

	public ProfileViewModel(IUserService userService, IDisciplineService disciplineService)
	{
		this._userService = userService;
		this._disciplineService = disciplineService;
	}

	[ObservableProperty]
	private string _name = string.Empty;
	[ObservableProperty]
	private string _surname = string.Empty;
	[ObservableProperty]
	private string _email = string.Empty;
	[ObservableProperty]
	private string _favouriteDiscipline = "Favourite Discipline";

	[ObservableProperty]
	private DateOnly _birthdate;

	[ObservableProperty]
	private string _boulderingBestGrade = "-";
	[ObservableProperty]
	private string _topRopeBestGrade = "-";
	[ObservableProperty]
	private string _leadBestGrade = "-";
	[ObservableProperty]
	private string _speedBestGrade = "-";

	public async Task LoadUserAsync()
	{
		try
		{
			var userEmail = Session.CurrentUserEmail;
			if (string.IsNullOrEmpty(userEmail))
			{
				await GoBackWithAlertAsync(
					message: "You are not logged in. Try to log in with your email!",
					pagePath: $"//{AppRoutes.LoginPage}",
					isError: true);
				return;
			}

			var user = await _userService.GetUserByEmailAsync(userEmail);

			if (user == null)
			{
				await GoBackWithAlertAsync(
					message: "We cannot find an account with your email. Try later or register!",
					pagePath: $"//{AppRoutes.LoginPage}",
					isError: true);
				return;
			}
			if (user.Id != Session.CurrentUserId)
			{
				await GoBackWithAlertAsync(
					message: "We cannot identify you. Try later or register!",
					pagePath: $"//{AppRoutes.LoginPage}",
					isError: true);
				return;
			}

			this.Name = user.Name;
			this.Surname = user.Surname;
			this.FavouriteDiscipline = user.FavouriteDiscipline?.Name ?? "Favourite Discipline";
			this.Email = user.Email;
			this.Birthdate = user.Birthday;

			var boulderingGradeTask = GetGradeAsync(Disciplines.Bouldering, user.Id);
			var topRopeGradeTask = GetGradeAsync(Disciplines.TopRopeClimbing, user.Id);
			var leadGradeTask = GetGradeAsync(Disciplines.LeadClimbing, user.Id);
			var speedGradeTask = GetGradeAsync(Disciplines.SpeedClimbing, user.Id);

			await Task.WhenAll(boulderingGradeTask, topRopeGradeTask, leadGradeTask, speedGradeTask);
			
			this.BoulderingBestGrade = boulderingGradeTask.Result;
			this.TopRopeBestGrade = topRopeGradeTask.Result;
			this.LeadBestGrade = leadGradeTask.Result;
			this.SpeedBestGrade = speedGradeTask.Result;
		}
		catch (KeyNotFoundException e)
		{
			await GoBackWithAlertAsync(
				message: "We cannot find an account with your email. Try later or to register!",
				pagePath: $"//{AppRoutes.LoginPage}",
				isError: true);
		}
		catch (Exception e)
		{
			await GoBackWithAlertAsync(
				message: "Couldn't load your profile page. Try later!",
				pagePath: $"//{AppRoutes.LoginPage}",
				isError: true);
		}
	}

	[RelayCommand]
	private void LogoutAndRedirectToLoginPage()
	{
		var loginPage = Application.Current.Handler?.MauiContext?.Services.GetService<LoginPage>();
		Application.Current.MainPage = loginPage;
		ClearSession();
	}

	[RelayCommand]
	private async Task SelectFavouriteDisciplineAsync()
	{
		var userId = Session.CurrentUserId;
		if (userId == null)
		{
			var loginPage = Application.Current.Handler?.MauiContext?.Services.GetService<LoginPage>();
			Application.Current.MainPage = loginPage;
			return;
		}
		
		string[] disciplineNames = [Disciplines.Bouldering, Disciplines.TopRopeClimbing, Disciplines.LeadClimbing,
			Disciplines.SpeedClimbing];
		
		var result = await Shell.Current.DisplayActionSheetAsync(
			"Select discipline", "Cancel", null, disciplineNames);

		if (result != null && result != "Cancel")
		{
			var selectedDiscipline = await _disciplineService.GetDisciplineByNameAsync(result);
			try
			{
				await _userService.EditUserInfoAsync(
					id: (int)userId,
					name: null,
					surname: null,
					birthday: null,
					favouriteDisciplineId: selectedDiscipline.Id);
				this.FavouriteDiscipline = result;
			}
			catch (ArgumentException e)
			{
				await Shell.Current.DisplayAlertAsync("Sorry,", 
					"Couldn't remember your favourite discipline. Try later!",
					"Ok");
			}
			catch (KeyNotFoundException e)
			{
				await Shell.Current.DisplayAlertAsync("Sorry,",
					"Couldn't find your favourite discipline. Try later!",
					"Ok");
			}
			catch (Exception e)
			{
				await Shell.Current.DisplayAlertAsync("Sorry,",
					"Couldn't write your favourite discipline for a mystery reason. Try later!",
					"Ok");
			}
		}
	}

	private async Task<string> GetGradeAsync(string disciplineName, int userId)
	{
		var discipline = await _disciplineService.GetDisciplineByNameAsync(disciplineName);
		var grade = await _userService.GetBestGradeInDisciplineAsync(userId, discipline.Id);
		return grade?.Name ?? "-";
	}
}
