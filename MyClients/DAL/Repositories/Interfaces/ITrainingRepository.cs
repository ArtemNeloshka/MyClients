using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public interface ITrainingRepository : IRepository<Training>
{
	Task<List<Training>> GetAllByUserIdAsync(int userId);
}