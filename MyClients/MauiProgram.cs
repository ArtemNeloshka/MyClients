using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.DAL;
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
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "myClients.db");

		builder.Services.AddDbContext<MyClientsDbContext>(options =>
			options.UseSqlite($"Filename={dbPath}"));
		
		// DAL
		builder.Services.AddTransient<IUserRepository, UserRepository>();
		builder.Services.AddTransient<IDisciplineRepository, DisciplineRepository>();
		builder.Services.AddTransient<IGradeRepository, GradeRepository>();
		builder.Services.AddTransient<ITrainingRepository, TrainingRepository>();
		builder.Services.AddTransient<IAttemptRepository, AttemptRepository>();
		builder.Services.AddTransient<IClimbResultRepository, ClimbResultRepository>();
		
		// BLL
		builder.Services.AddTransient<IUserService, UserService>();
		builder.Services.AddTransient<IDisciplineService, DisciplineService>();
		builder.Services.AddTransient<IGradeService, GradeService>();
		builder.Services.AddTransient<ITrainingService, TrainingService>();
		
		// PL
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddTransient<RegistrationPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DisciplinesPage>();
		builder.Services.AddTransient<TrainPage>();
		builder.Services.AddTransient<StatisticsPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<WorkoutsArchivePage>();
		builder.Services.AddTransient<TrainingDetailsPage>();
		
		// viewModels
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<LogInViewModel>();
		builder.Services.AddSingleton<TrainPageViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<DisciplinesViewModel>();
		builder.Services.AddTransient<WorkoutsArchiveViewModel>();
		builder.Services.AddTransient<TrainingDetailsViewModel>();
		builder.Services.AddTransient<MainViewModel>();
		
		builder.Services.AddSingleton<AppShell>();
	
		var app = builder.Build();
		
#if DEBUG
		using var scope = app.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<MyClientsDbContext>();

		// dbContext.Database.EnsureDeleted();
		dbContext.Database.EnsureCreated(); 
		
		Task.Run(async () => await dbContext.SeedTestDataAsync()).Wait();
#endif
		return app;
	}
}
