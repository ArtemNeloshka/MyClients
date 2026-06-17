using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class GradeRepository : Repository<Grade>, IGradeRepository
{
	public GradeRepository(MyClientsDbContext context) : base(context) 
	{
		
	}
}