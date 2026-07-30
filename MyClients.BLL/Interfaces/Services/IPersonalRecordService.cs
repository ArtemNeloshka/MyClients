using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Services;

public interface IPersonalRecordService
{
	Task<PersonalRecord?> GetRecordByIdAsync(int id);
	Task<ICollection<PersonalRecord>> GetRecordsByUserIdAsync(int userId);
	Task AddRecordAsync(User user, Grade grade, DateOnly date, Discipline discipline);
	Task UpdateRecordAsync(int id, Grade? grade, DateOnly? date);
}