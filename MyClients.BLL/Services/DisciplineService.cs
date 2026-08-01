using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class DisciplineService : IDisciplineService
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

	public async Task<Discipline> GetDisciplineByNameAsync(string name)
	{
		var discipline = await _disciplineRepository.GetDisciplineByNameAsync(name);

		if (discipline == null)
		{
			throw new KeyNotFoundException(ErrorMessages.DisciplineNotFoundMessage);
		}

		return discipline;
	}
}