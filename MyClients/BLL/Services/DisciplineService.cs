using MyClients.BLL.Interfaces;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;

namespace MyClients.BLL.Services;

public class DisciplineService : Service, IDisciplineService
{
	private readonly IDisciplineRepository _disciplineRepository;

	public DisciplineService(IDisciplineRepository disciplineRepository)
	{
		this._disciplineRepository = disciplineRepository;
	}
	
	public async Task<Discipline?> GetDisciplineByIdAsync(int id)
	{
		return await _disciplineRepository.GetByIdAsync(id);
	}

	public async Task<ICollection<Discipline>> GetAllDisciplinesAsync()
	{
		var disciplines = (await _disciplineRepository.GetAllAsync()).ToList();

		return disciplines;
	}
}