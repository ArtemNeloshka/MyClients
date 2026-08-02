using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class GradeService : IGradeService
{
	private readonly IGradeRepository _gradeRepository;

	public GradeService(IGradeRepository gradeRepository)
	{
		this._gradeRepository = gradeRepository;
	}
	
	public async Task<Grade> GetGradeByIdAsync(int id)
	{
		var grade = await _gradeRepository.GetByIdAsync(id);

		if (grade == null)
		{
			throw new KeyNotFoundException(ErrorMessages.GradeNotFound);
		}

		return grade;
	}

	public async Task<ICollection<Grade>> GetAllGradesAsync()
	{
		return (await _gradeRepository.GetAllAsync()).ToList();
	}
}
