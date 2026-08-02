using Microsoft.EntityFrameworkCore;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.Domain.Entities;
using static MyClients.Domain.Constants.ClimbResults;

namespace MyClients.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
	public UserRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<Grade?> GetBestGradeInDisciplineAsync(int userId, int disciplineId)
	{
		return await _dbContext.Attempts
			.Where(a => a.DisciplineId == disciplineId
			            && a.Training.UserId == userId
			            && (string.Equals(a.ClimbResult.Name, Flash)
			                || string.Equals(a.ClimbResult.Name, Top)))
			.Select(a => a.Grade)
			.OrderByDescending(grade => grade.Value)
			.FirstOrDefaultAsync();
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		return await _dbSet.FirstOrDefaultAsync(u => string.Equals(u.Email.ToLower(), email.ToLower()));
	}
}
