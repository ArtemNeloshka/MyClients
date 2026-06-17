using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
	public UserRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}
