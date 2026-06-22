using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public interface IPersonalRecordRepository : IRepository<PersonalRecord>
{
	Task<IEnumerable<PersonalRecord>> GetRecordsByUserIdAsync(int userId);
}