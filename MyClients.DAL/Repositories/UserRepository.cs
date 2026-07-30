using Microsoft.EntityFrameworkCore;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.Domain.Entities;

namespace MyClients.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
	public UserRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<Grade> GetBestGradeInDisciplineAsync(int userId, int disciplineId)
	{
		var bestGrade = await _dbContext.PersonalRecords
			.Where(pr => pr.UserId == userId
			             && pr.DisciplineId == disciplineId)
			.Select(pr => pr.Grade)
			.OrderByDescending(grade => grade.Value)
			.FirstOrDefaultAsync();

		return bestGrade ?? new Grade{Value = 0, Name = "-",};
	}
}
