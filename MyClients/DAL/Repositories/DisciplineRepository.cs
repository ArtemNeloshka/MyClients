using Microsoft.EntityFrameworkCore;
using MyClients.DAL.Entities;
using MyClients.DAL;

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
