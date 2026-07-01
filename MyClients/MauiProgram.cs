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
		builder.Services.AddTransient<MainViewModel>();
		
		builder.Services.AddSingleton<AppShell>();
	
		var app = builder.Build();
		
		return app;
	}
}