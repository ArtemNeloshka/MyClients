using Microsoft.Extensions.Logging;
using MyClients.DAL;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;

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

		builder.Services.AddDbContext<MyClientsDbContext>();

		builder.Services.AddTransient<IUserRepository, UserRepository>();
		builder.Services.AddTransient<IDisciplineRepository, DisciplineRepository>();
		builder.Services.AddTransient<IGradeRepository, GradeRepository>();
		builder.Services.AddTransient<ITrainingRepository, TrainingRepository>();
		builder.Services.AddTransient<IPersonalRecordRepository, PersonalRecordRepository>();
	
		var app = builder.Build();

		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<MyClientsDbContext>();
			db.Database.EnsureCreated();
		}
		
		return app;
	}
}