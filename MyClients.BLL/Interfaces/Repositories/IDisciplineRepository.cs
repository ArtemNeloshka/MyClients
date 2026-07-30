using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface IDisciplineRepository : IRepository<Discipline>
{
	Task<Discipline?> GetDisciplineByNameAsync(string name);
}