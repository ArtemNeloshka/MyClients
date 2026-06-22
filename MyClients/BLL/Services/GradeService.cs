using MyClients.BLL.Interfaces;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;

namespace MyClients.BLL.Services;

public class GradeService : Service, IGradeService
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