using MyClients.BLL.Interfaces.Repositories;
using MyClients.Domain.Entities;

namespace MyClients.DAL.Repositories;

public class GradeRepository : Repository<Grade>, IGradeRepository
{
	public GradeRepository(MyClientsDbContext context) : base(context) 
	{
		
	}
}
