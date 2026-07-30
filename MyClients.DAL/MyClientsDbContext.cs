using Microsoft.EntityFrameworkCore;
using MyClients.Domain.Entities;

namespace MyClients.DAL;

public class MyClientsDbContext : DbContext
{
	// Entities
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Training> Trainings { get; set; } = null!;
	public DbSet<Grade> Grades { get; set; } = null!;
	public DbSet<Discipline> Disciplines { get; set; } = null!;
	public DbSet<PersonalRecord> PersonalRecords { get; set; } = null!;

	public MyClientsDbContext(DbContextOptions options) : base(options)
	{
		
	}
	
	public async Task SeedTestDataAsync()
	{
        
	    if (this.Users.Any())
	        return;

	    var user = new User
	    {
	        Name = "Test",
	        Surname = "Test",
	        Birthday = new DateOnly(1995, 5, 10),
	        Email = "test@gmail.com",
	        PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
	    };
	    
	    await this.Users.AddAsync(user);
	    await this.SaveChangesAsync();

	    var bouldering = new Discipline 
	    { 
	        Name = "Bouldering", 
	        Description = "Короткі складні траси без мотузки над матами." 
	    };
	    var lead = new Discipline 
	    { 
	        Name = "Lead Climbing", 
	        Description = "Довгі траси з нижньою страховкою." 
	    };
	    var topRope = new Discipline 
	    { 
		    Name = "Top Rope Climbing", 
		    Description = "Довгі траси з верхньою страховкою." 
	    };
	    var speed = new Discipline 
	    { 
		    Name = "Speed Climbing", 
		    Description = "Лазіння по еталонному маршруту висотою 15м на час." 
	    };
	    
	    await this.Disciplines.AddRangeAsync(bouldering, topRope, lead, speed);
	    await this.SaveChangesAsync();

	    var grade6A = new Grade { Name = "6A", Value = 10 };
	    var grade6B = new Grade { Name = "6B", Value = 12 };
	    var grade6C = new Grade { Name = "6C", Value = 14 };
	    
	    await this.Grades.AddRangeAsync(grade6A, grade6B, grade6C);
	    await this.SaveChangesAsync();

	    var today = DateOnly.FromDateTime(DateTime.Now);

	    var testTrainings = new List<Training>
	    {
	        new Training 
	        { 
	            UserId = user.Id, 
	            TrainingDate = today.AddDays(-5), 
	            TrainingLog = "Гарна розминка. Лазив переважно боулдеринг категорії 6A. Пальці швидко втомилися."
	        },
	        new Training 
	        { 
	            UserId = user.Id, 
	            TrainingDate = today.AddDays(-2), 
	            TrainingLog = "Робота над проєктом 6B на нависання. Зробив 5 спроб, розклав усі рухи, але не зібрав до купи."
	        },
	        new Training 
	        { 
	            UserId = user.Id, 
	            TrainingDate = today, 
	            TrainingLog = "Відновлювальне тренування на трудність. Багато об'єму на легких трасах."
	        }
	    };

	    await this.Trainings.AddRangeAsync(testTrainings);

	    var pr = new PersonalRecord
	    {
	        UserId = user.Id,
	        DisciplineId = bouldering.Id,
	        GradeId = grade6B.Id,
	        RecordDate = today.AddDays(-2)
	    };

	    await this.PersonalRecords.AddAsync(pr);
	    
	    await this.SaveChangesAsync();
	}
}