using MyClients.DAL.Entities;

namespace MyClients.DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
	public Task<Grade> GetBestGradeInDisciplineAsync(int userId, int disciplineId);
}