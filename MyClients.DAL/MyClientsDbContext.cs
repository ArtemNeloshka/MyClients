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
	public DbSet<Attempt> Attempts { get; set; } = null!;
	public DbSet<ClimbResult> ClimbResults { get; set; } = null!;
	
	public MyClientsDbContext(DbContextOptions<MyClientsDbContext> options) : base(options)
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


	    var flash = new ClimbResult
	    {
		    Name = "Flash",
		    Description = "Sending from the first time",
	    };

	    var top = new ClimbResult
	    {
		    Name = "Top",
		    Description = "Not from the first time, but sending",
	    };

	    var zone = new ClimbResult
	    {
		    Name = "Zone",
		    Description = "Climbing to the zone, more competition thing",
	    };

	    var fail = new ClimbResult
	    {
		    Name = "Fail",
		    Description = "You failed, you're loder. Try one more time!",
	    };

	    await this.ClimbResults.AddAsync(flash);
	    await this.ClimbResults.AddAsync(top);
	    await this.ClimbResults.AddAsync(zone);
	    await this.ClimbResults.AddAsync(fail);
	    await this.SaveChangesAsync();

	    var today = DateOnly.FromDateTime(DateTime.Now);

	    var training1 = new Training
	    {
		    UserId = user.Id,
		    TrainingDate = today.AddDays(-5),
		    TrainingDuration = new TimeSpan(hours: 4, minutes: 0, seconds: 0),
		    TrainingLog = "Гарна розминка. Лазив переважно боулдеринг категорії 6A. Пальці швидко втомилися."
	    };
	    
	    var training2 = new Training
	    {
		    UserId = user.Id,
		    TrainingDate = today.AddDays(-2),
		    TrainingDuration = new TimeSpan(hours: 4, minutes: 0, seconds: 0),
		    TrainingLog =
			    "Робота над проєктом 6B на нависання. Зробив 5 спроб, розклав усі рухи, але не зібрав до купи."
	    };

	    var training3 = new Training
	    {
		    UserId = user.Id,
		    TrainingDate = today,
		    TrainingDuration = new TimeSpan(hours: 4, minutes: 0, seconds: 0),
		    TrainingLog = "Відновлювальне тренування на трудність. Багато об'єму на легких трасах."
	    };

	    await this.Trainings.AddAsync(training1);
	    await this.Trainings.AddAsync(training2);
	    await this.Trainings.AddAsync(training3);
	    await this.SaveChangesAsync();
	    
	    var attemptTraining1Flash = new Attempt
	    {
		    TrainingId = training1.Id,
		    DisciplineId = bouldering.Id,
		    GradeId = grade6A.Id,
		    ClimbResultId = flash.Id,
		    Timestamp = new TimeSpan(hours: 0, minutes: 25, seconds: 0),
	    };
	    
	    var attemptTraining1Fail = new Attempt
	    {
		    TrainingId = training1.Id,
		    DisciplineId = bouldering.Id,
		    GradeId = grade6C.Id,
		    ClimbResultId = fail.Id,
		    Timestamp = new TimeSpan(hours: 1, minutes: 25, seconds: 0),
	    };
	    
	    var attemptTraining2Top = new Attempt
	    {
		    TrainingId = training2.Id,
		    DisciplineId = lead.Id,
		    GradeId = grade6B.Id,
		    ClimbResultId = top.Id,
		    Timestamp = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
	    };
	    
	    var attemptTraining3Top = new Attempt
	    {
		    TrainingId = training3.Id,
		    DisciplineId = topRope.Id,
		    GradeId = grade6B.Id,
		    ClimbResultId = top.Id,
		    Timestamp = new TimeSpan(hours: 3, minutes: 25, seconds: 0),
	    };
	    
	    var attemptTraining3Zone = new Attempt
	    {
		    TrainingId = training3.Id,
		    DisciplineId = bouldering.Id,
		    GradeId = grade6C.Id,
		    ClimbResultId = zone.Id,
		    Timestamp = new TimeSpan(hours: 0, minutes: 25, seconds: 0),
	    };

	    await this.Attempts.AddAsync(attemptTraining1Fail);
	    await this.Attempts.AddAsync(attemptTraining1Flash);
	    await this.Attempts.AddAsync(attemptTraining2Top);
	    await this.Attempts.AddAsync(attemptTraining3Top);
	    await this.Attempts.AddAsync(attemptTraining3Zone);
	    await this.SaveChangesAsync();
	}
}
