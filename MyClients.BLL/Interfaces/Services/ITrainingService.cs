using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Services;

public interface ITrainingService
{
	// CRUD
	// Create
	Task CreateTrainingAsync(int userId, DateOnly trainingDate, TimeSpan duration, string? trainingLog,
		ICollection<Attempt> attempts);
	// Read
	Task<Training?> GetTrainingByIdAsync(int id);
	Task<ICollection<Training>> GetTrainingsByUserIdAsync(int userId);
	Task<ICollection<Training>> GetTrainingsByPeriodAsync(int userId, DateOnly start, DateOnly end);
	// Update
	Task EditTrainingLogAsync(int id, string text);
	// Delete
	Task DeleteTrainingAsync(int id);
	
	// Attempts
	Task<ICollection<Attempt>> GetAllAttemptsByTrainingIdAsync(int trainingId);
	Task<ICollection<Attempt>> GetTopAttemptsByTrainingIdAsync(int trainingId, int amount);

	// Disciplines
	Task<ICollection<Discipline>> GetAllDisciplinesByTrainingIdAsync(int trainingId);
}
