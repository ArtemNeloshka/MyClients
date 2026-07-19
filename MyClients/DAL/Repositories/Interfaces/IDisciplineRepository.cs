using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public interface IDisciplineRepository : IRepository<Discipline>
{
	Task<Discipline?> GetDisciplineByNameAsync(string name);
}