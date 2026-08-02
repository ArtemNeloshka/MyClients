using Microsoft.EntityFrameworkCore;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.Domain.Entities;

namespace MyClients.DAL.Repositories;

public class TrainingRepository : Repository<Training>, ITrainingRepository
{
	public TrainingRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<List<Training>> GetAllByUserIdAsync(int userId)
	{
		return await _dbSet
			.Where(t => t.UserId == userId)
			.ToListAsync();
	}

	public async Task<List<Training>> GetTrainingsByPeriodAsync(int userId, DateOnly start, DateOnly end)
	{
		return await _dbSet
			.Where(t => t.UserId == userId 
			            && t.TrainingDate >= start
			            && t.TrainingDate <= end)
			.ToListAsync();
	}

	public async Task<List<Attempt>> GetAllAttemptsByTrainingIdAsync(int trainingId)
	{
		return await _dbContext.Attempts
			.Include(a => a.Discipline)
			.Include(a => a.Grade)
			.Include(a => a.ClimbResult)
			.Where(a => a.TrainingId == trainingId)
			.ToListAsync();
	}

	public async Task<List<Attempt>> GetTopAttemptsByTrainingIdAsync(int trainingId, int amount)
	{
		return await _dbContext.Attempts
			.Include(a => a.Discipline)
			.Include(a => a.Grade)
			.Include(a => a.ClimbResult)
			.Where(a => a.TrainingId == trainingId)
			.OrderByDescending(a => a.Grade.Value)
			.Take(amount)
			.ToListAsync();
	}

	public async Task<List<Discipline>> GetAllDisciplinesByTrainingIdAsync(int trainingId)
	{
		return await _dbContext.Attempts
			.Where(a => a.TrainingId == trainingId)
			.Select(a => a.Discipline)
			.Distinct()
			.ToListAsync();
	}
}
