using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface IAttemptRepository : IRepository<Attempt>
{
	Task<List<Attempt>> GetAllByTrainingIdAsync(int trainingId);
	Task<List<Attempt>> GetBestsByTrainingIdAsync(int trainingId, int amount);
}