using Microsoft.EntityFrameworkCore;
using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
	public UserRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<Grade> GetBestGradeInDisciplineAsync(int userId, int disciplineId)
	{
		var bestGrade = await _dbContext.Attempts
			.Where(a => a.DisciplineId == disciplineId
			            && a.Training.UserId == userId
			            && (a.ClimbResult.Name == "Flash"
			                || a.ClimbResult.Name == "Top"))
			.Select(a => a.Grade)
			.OrderByDescending(grade => grade.Value)
			.FirstOrDefaultAsync();

		return bestGrade ?? new Grade{Value = 0, Name = "-",};
	}
}
