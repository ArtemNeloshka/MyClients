using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class AttemptRepository : Repository<Attempt>, IAttemptRepository
{
	public AttemptRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}