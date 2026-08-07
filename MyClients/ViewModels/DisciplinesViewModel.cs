using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Entities;

namespace MyClients.ViewModels;

public class DisciplinesViewModel : ObservableObject
{
	private readonly IDisciplineService _disciplineService;

	public DisciplinesViewModel(IDisciplineService disciplineService)
	{
		this._disciplineService = disciplineService;
	}
	
	public ObservableCollection<Discipline> Disciplines { get; set; } = new ();

	public async Task LoadDisciplinesAsync()
	{
		try
		{
			var disciplines = await _disciplineService.GetAllDisciplinesAsync();

			MainThread.BeginInvokeOnMainThread(() =>
			{
				Disciplines.Clear();
				foreach (var d in disciplines)
					Disciplines.Add(d);
			});
		}
		catch (KeyNotFoundException e)
		{
			Console.WriteLine(e);
			await Shell.Current.DisplayAlertAsync("Sorry,", "We don't have any disciplines yet. Try later!", "Ok");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			await Shell.Current.DisplayAlertAsync("Sorry,", "Something wrong happened. Try later!", "Ok");
		}
	}
}
