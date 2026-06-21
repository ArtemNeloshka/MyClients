using MyClients.BLL.Services;
using MyClients.DAL.Entities;

namespace MyClients.BLL.Interfaces;

public interface ITrainingService
{
	Task AddTrainingLogAsync(int id, string text);
	Task EditTrainingLogAsync(int id, string text);
	Task DeleteTrainingAsync(int id);
	Task<Training> GetTrainingByIdAsync(int id);
	Task<ICollection<Training>> GetTrainingsByPeriodAsync(DateOnly start, DateOnly end);
}