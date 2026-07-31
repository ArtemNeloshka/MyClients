using MyClients.Domain.Entities;
using MyClients.BLL.Interfaces.Repositories;

namespace MyClients.DAL.Repositories;

public class AttemptRepository : Repository<Attempt>, IAttemptRepository
{
	public AttemptRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}