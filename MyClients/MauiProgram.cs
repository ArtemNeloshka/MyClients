using Microsoft.Extensions.Logging;
using MyClients.BLL.Interfaces;
using MyClients.BLL.Services;
using MyClients.DAL;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;
using MyClients.ViewModels;
using MyClients.Views;

namespace MyClients;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<App>();
		
		// db context
		builder.Services.AddDbContext<MyClientsDbContext>();
		
		// DAL
		builder.Services.AddTransient<IUserRepository, UserRepository>();
		builder.Services.AddTransient<IDisciplineRepository, DisciplineRepository>();
		builder.Services.AddTransient<IGradeRepository, GradeRepository>();
		builder.Services.AddTransient<ITrainingRepository, TrainingRepository>();
		builder.Services.AddTransient<IPersonalRecordRepository, PersonalRecordRepository>();
		
		// BLL
		builder.Services.AddTransient<IUserService, UserService>();
		builder.Services.AddTransient<IDisciplineService, DisciplineService>();
		builder.Services.AddTransient<IGradeService, GradeService>();
		builder.Services.AddTransient<ITrainingService, TrainingService>();
		builder.Services.AddTransient<IPersonalRecordService, PersonalRecordService>();
		
		// PL
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegistrationPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DisciplinesPage>();
		builder.Services.AddTransient<TrainPage>();
		builder.Services.AddTransient<StatisticsPage>();
		builder.Services.AddTransient<ProfilePage>();
		
		// viewModels
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<LogInViewModel>();
		builder.Services.AddSingleton<TrainPageViewModel>();
		builder.Services.AddTransient<MainViewModel>();
		
		builder.Services.AddSingleton<AppShell>();
	
		var app = builder.Build();
		
		// test data
		
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<MyClientsDbContext>();
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();

			if (!db.Grades.Any())
			{
				db.Grades.AddRange(
					new Grade { Name = "5a", Value = 1 },
					new Grade { Name = "5b", Value = 2 },
					new Grade { Name = "5c", Value = 3 },
					new Grade { Name = "6a", Value = 4 },
					new Grade { Name = "6a+", Value = 5 },
					new Grade { Name = "6b", Value = 6 },
					new Grade { Name = "6b+", Value = 7 },
					new Grade { Name = "6c", Value = 8 },
					new Grade { Name = "6c+", Value = 9 },
					new Grade { Name = "7a", Value = 10 },
					new Grade { Name = "7a+", Value = 11 },
					new Grade { Name = "7b", Value = 12 }
				);

				db.Disciplines.AddRange(
					new Discipline
					{
						Name = "Bouldering",
						Description =
							"Bouldering is a form of free climbing performed on small rock formations or artificial rock walls without ropes or harnesses."
					},
					new Discipline
					{
						Name = "Top Rope Climbing",
						Description =
							"Top rope climbing is a style of climbing where the rope runs from a belayer up through an anchor at the top of the route and back down to the climber."
					},
					new Discipline
					{
						Name = "Lead Climbing",
						Description =
							"Lead climbing is a technique where the climber clips the rope to protection points as they ascend the route."
					},
					new Discipline
					{
						Name = "Speed Climbing",
						Description =
							"Speed climbing is a racing discipline where climbers race head-to-head on a standardized 15-meter wall."
					}
				);

				db.SaveChanges();

				var user = new User
				{
					Name = "Artem",
					Surname = "Test",
					Email = "artem@gmail.com",
					Birthday = new DateOnly(2000, 1, 1),
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678")
				};
				db.Users.Add(user);
				db.SaveChanges();

				var bouldering = db.Disciplines.First(d => d.Name == "Bouldering");
				var topRope = db.Disciplines.First(d => d.Name == "Top Rope Climbing");
				var lead = db.Disciplines.First(d => d.Name == "Lead Climbing");

				var grade7a = db.Grades.First(g => g.Name == "7a");
				var grade6b = db.Grades.First(g => g.Name == "6b");
				var grade6c = db.Grades.First(g => g.Name == "6c+");

				db.Trainings.AddRange(
					new Training
					{
						UserId = user.Id, TrainingDate = new DateOnly(2025, 6, 1),
						TrainingLog = "Good session, felt strong on the overhang."
					},
					new Training
					{
						UserId = user.Id, TrainingDate = new DateOnly(2025, 6, 3),
						TrainingLog = "Worked on footwork and balance."
					},
					new Training
					{
						UserId = user.Id, TrainingDate = new DateOnly(2025, 6, 5),
						TrainingLog = "Tough day, arms were pumped."
					}
				);

				db.PersonalRecords.AddRange(
					new PersonalRecord
					{
						UserId = user.Id, DisciplineId = bouldering.Id, GradeId = grade7a.Id,
						RecordDate = new DateOnly(2025, 3, 10)
					},
					new PersonalRecord
					{
						UserId = user.Id, DisciplineId = topRope.Id, GradeId = grade6c.Id,
						RecordDate = new DateOnly(2025, 4, 15)
					},
					new PersonalRecord
					{
						UserId = user.Id, DisciplineId = lead.Id, GradeId = grade6b.Id,
						RecordDate = new DateOnly(2025, 5, 20)
					}
				);

				db.SaveChanges();
			}
		}
	
		return app;
	}
}