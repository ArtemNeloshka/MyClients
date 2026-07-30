using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface ITrainingRepository : IRepository<Training>
{
	Task<List<Training>> GetAllByUserIdAsync(int userId);
}