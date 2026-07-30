using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class TrainingService : Service, ITrainingService
{
	private readonly ITrainingRepository _trainingRepository;

	public TrainingService(ITrainingRepository trainingRepository)
	{
		this._trainingRepository = trainingRepository;
	}
	
	public async Task AddTrainingLogAsync(int id, string text)
	{
		// input validation
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new ArgumentException(ErrorMessages.TrainingLogIsNullOrEmpty, nameof(text));
		}

		// get a record from DB
		var training = await _trainingRepository.GetByIdAsync(id);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={id})");
		}
		
		// adding the log
		training.TrainingLog += $"\n[{DateTime.Now:HH:mm:ss}]: {text}";

		await _trainingRepository.UpdateAsync(training);
	}

	public async Task EditTrainingLogAsync(int id, string text)
	{
		// input validation
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new ArgumentException(ErrorMessages.TrainingLogIsNullOrEmpty, nameof(text));
		}
		
		// get a record from DB
		var training = await _trainingRepository.GetByIdAsync(id);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={id})");
		}
		
		// editing the log
		training.TrainingLog = text;

		await _trainingRepository.UpdateAsync(training);
	}

	public async Task DeleteTrainingAsync(int id)
	{
		// get record from DB
		var training = await _trainingRepository.GetByIdAsync(id);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={id})");
		}
		
		// deleting training
		await _trainingRepository.DeleteAsync(training);
	}

	public async Task<Training> GetTrainingByIdAsync(int id)
	{
		var training = await _trainingRepository.GetByIdAsync(id);
		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={id})");
		}

		return training;
	}

	public async Task<ICollection<Training>> GetTrainingsByPeriodAsync(DateOnly start, DateOnly end)
	{
		// validate input
		if (end > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday, nameof(end));
		}

		if (start > end)
		{
			throw new ArgumentException(ErrorMessages.DateStartHigherThanEnd);
		}
		
		// getting record from DB
		var allTrainings = await _trainingRepository.GetAllAsync();

		var selectedTrainings = allTrainings
			.Where(t => t.TrainingDate >= start && t.TrainingDate <= end)
			.ToList();

		return selectedTrainings;
	}

	public async Task<ICollection<Training>> GetTrainingsByUserIdAsync(int userId)
	{
		var trainings = await _trainingRepository.GetAllByUserIdAsync(userId);

		return trainings;
	}
}