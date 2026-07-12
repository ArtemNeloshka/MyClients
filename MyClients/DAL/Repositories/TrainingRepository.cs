using Microsoft.EntityFrameworkCore;
using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class TrainingRepository : Repository<Training>, ITrainingRepository
{
	public TrainingRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<List<Training>> GetAllByUserIdAsync(int userId)
	{
		var trainings = await _dbContext.Trainings
			.Where(t => t.UserId == userId)
			.ToListAsync();

		return trainings;
	}
}