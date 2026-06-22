using Microsoft.EntityFrameworkCore;
using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class PersonalRecordRepository : Repository<PersonalRecord>, IPersonalRecordRepository
{
	public PersonalRecordRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<IEnumerable<PersonalRecord>> GetRecordsByUserIdAsync(int userId)
	{
		return await _dbSet
			.Where(pr => pr.UserId == userId)
			.ToListAsync();
	}
}