using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class PersonalRecordRepository : Repository<PersonalRecord>, IPersonalRecordRepository
{
	public PersonalRecordRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}