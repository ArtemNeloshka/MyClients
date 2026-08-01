using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class GradeService : IGradeService
{
	private readonly IGradeRepository _gradeRepository;

	public GradeService(IGradeRepository gradeRepository)
	{
		this._gradeRepository = gradeRepository;
	}
	
	public async Task<Grade?> GetGradeByIdAsync(int id)
	{
		return await _gradeRepository.GetByIdAsync(id);
	}

	public async Task<ICollection<Grade>> GetAllGradesAsync()
	{
		var grades = await _gradeRepository.GetAllAsync();

		return grades.ToList();
	}
}