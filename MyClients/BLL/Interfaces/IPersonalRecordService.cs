using MyClients.BLL.Services;
using MyClients.DAL.Entities;

namespace MyClients.BLL.Interfaces;

public interface IPersonalRecordService
{
	Task<PersonalRecord> GetRecordByIdAsync(int id);
	Task<ICollection<PersonalRecord>> GetRecordByUserIdAsync(int userId);
	Task AddRecordAsync(User user, Grade grade, DateOnly date, Discipline discipline);
	Task UpdateRecordAsync(int id, Grade? grade, DateOnly? date);
}