using MyClients.DAL.Entities;
using MyClients.DAL;

namespace MyClients.DAL.Repositories;

public class DisciplineRepository : Repository<Discipline>, IDisciplineRepository
{
	public DisciplineRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}
