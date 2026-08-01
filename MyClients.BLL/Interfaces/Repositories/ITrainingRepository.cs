using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface ITrainingRepository : IRepository<Training>
{
	Task<List<Training>> GetAllByUserIdAsync(int userId);
	Task<List<Training>> GetTrainingsByPeriodAsync(int userId, DateOnly start, DateOnly end);
	Task<List<Attempt>> GetAllAttemptsByTrainingIdAsync(int trainingId);
	Task<List<Attempt>> GetTopAttemptsByTrainingIdAsync(int trainingId, int amount);
	Task<List<Discipline>> GetAllDisciplinesByTrainingIdAsync(int trainingId);
}
