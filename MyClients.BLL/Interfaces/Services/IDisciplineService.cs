using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Services;

public interface IDisciplineService
{
	Task<Discipline> GetDisciplineByIdAsync(int id);
	Task<ICollection<Discipline>> GetAllDisciplinesAsync();
	Task<Discipline> GetDisciplineByNameAsync(string name);
}