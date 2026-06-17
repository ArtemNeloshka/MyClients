using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class TrainingRepository : Repository<Training>, ITrainingRepository
{
	public TrainingRepository(MyClientsDbContext context) : base(context)
	{
		
	}
}