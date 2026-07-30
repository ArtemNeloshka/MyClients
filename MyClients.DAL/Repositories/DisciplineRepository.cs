using Microsoft.EntityFrameworkCore;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.DAL;
using MyClients.Domain.Entities;

namespace MyClients.DAL.Repositories;

public class DisciplineRepository : Repository<Discipline>, IDisciplineRepository
{
	public DisciplineRepository(MyClientsDbContext context) : base(context)
	{
		
	}

	public async Task<Discipline?> GetDisciplineByNameAsync(string name)
	{
		return await _dbSet.FirstOrDefaultAsync(d => d.Name == name);
	}
}
