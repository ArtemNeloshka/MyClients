using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Services;

public interface IGradeService
{
	Task<Grade> GetGradeByIdAsync(int id);
	Task<ICollection<Grade>> GetAllGradesAsync();
}