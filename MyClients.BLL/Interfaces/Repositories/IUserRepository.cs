using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
	public Task<Grade?> GetBestGradeInDisciplineAsync(int userId, int disciplineId);
}