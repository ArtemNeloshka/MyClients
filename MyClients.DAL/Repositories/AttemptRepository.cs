using Microsoft.EntityFrameworkCore;
using MyClients.Domain.Entities;
using MyClients.BLL.Interfaces.Repositories;

namespace MyClients.DAL.Repositories;

public class AttemptRepository : Repository<Attempt>, IAttemptRepository
{
	public AttemptRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<List<Attempt>> GetAllByTrainingIdAsync(int trainingId)
	{
		return await _dbContext.Attempts
			.Include(a => a.Grade)
			.Include(a => a.Discipline)
			.Include(a => a.ClimbResult)
			.Where(a => a.TrainingId == trainingId)
			.ToListAsync();
	}

	public async Task<List<Attempt>> GetBestsByTrainingIdAsync(int trainingId, int amount)
	{
		return await _dbContext.Attempts
			.Include(a => a.Grade)
			.Include(a => a.Discipline)
			.Include(a => a.ClimbResult)
			.Where(a => a.TrainingId == trainingId)
			.OrderByDescending(a => a.Grade.Value)
			.Take(amount)
			.ToListAsync();
	}
}