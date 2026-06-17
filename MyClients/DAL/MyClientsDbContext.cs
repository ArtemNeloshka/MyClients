using Microsoft.EntityFrameworkCore;
using MyClients.DAL.Entities;

namespace MyClients.DAL;

public class MyClientsDbContext : DbContext
{
	// Entities
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Training> Trainings { get; set; } = null!;
	public DbSet<Grade> Grades { get; set; } = null!;
	public DbSet<Discipline> Disciplines { get; set; } = null!;
	public DbSet<PersonalRecord> PersonalRecords { get; set; } = null!;
	
	// Connections
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// Protected folder for device
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myclients.db3");

		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}
}