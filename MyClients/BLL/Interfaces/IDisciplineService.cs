using MyClients.DAL.Entities;

namespace MyClients.BLL.Interfaces;

public interface IDisciplineService
{
	Task<Discipline?> GetDisciplineByIdAsync(int id);
	Task<ICollection<Discipline>> GetAllDisciplinesAsync();
	Task<Discipline> GetDisciplineByNameAsync(string name);
}