using MyClients.Domain.Entities;
using MyClients.BLL.Interfaces.Repositories;

namespace MyClients.DAL.Repositories;

public class ClimbResultRepository : Repository<ClimbResult>, IClimbResultRepository
{
	public ClimbResultRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}