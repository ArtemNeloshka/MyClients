using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class ClimbResultRepository : Repository<ClimbResult>, IClimbResultRepository
{
	public ClimbResultRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}