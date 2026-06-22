using MyClients.DAL.Entities;

namespace MyClients.BLL.Interfaces;

public interface IGradeService
{
	Task<Grade?> GetGradeByIdAsync(int id);
	Task<ICollection<Grade>> GetAllGradesAsync();
}