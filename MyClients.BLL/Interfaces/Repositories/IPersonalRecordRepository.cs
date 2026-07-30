using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface IPersonalRecordRepository : IRepository<PersonalRecord>
{
	Task<IEnumerable<PersonalRecord>> GetRecordsByUserIdAsync(int userId);
}