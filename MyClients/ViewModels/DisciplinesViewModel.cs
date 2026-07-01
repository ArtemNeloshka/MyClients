using System.Collections.ObjectModel;
using MyClients.BLL.Interfaces;
using MyClients.DAL.Entities;

namespace MyClients.ViewModels;

public class DisciplinesViewModel
{
	private readonly IDisciplineService _disciplineService;

	public DisciplinesViewModel(IDisciplineService disciplineService)
	{
		this._disciplineService = disciplineService;
	}
	
	public ObservableCollection<Discipline> Disciplines { get; set; } = new ();

	public async Task LoadDisciplinesAsync()
	{
		var disciplines = await _disciplineService.GetAllDisciplinesAsync();
		Disciplines.Clear();
        foreach (var d in disciplines)
            Disciplines.Add(d);
	}
}