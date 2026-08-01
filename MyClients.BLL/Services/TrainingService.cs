using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class TrainingService : ITrainingService
{
	private readonly ITrainingRepository _trainingRepository;

	public TrainingService(ITrainingRepository trainingRepository)
	{
		this._trainingRepository = trainingRepository;
	}

	public async Task CreateTrainingAsync(int userId, DateOnly trainingDate, TimeSpan duration, string? trainingLog,
		ICollection<Attempt> attempts)
	{
		if (trainingDate > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday, nameof(trainingDate));
		}
		if (duration < TimeSpan.Zero)
		{
			throw new ArgumentException(ErrorMessages.InvalidDuration, nameof(duration));
		}

		trainingLog = trainingLog?.Trim();

		if (trainingLog?.Length > ValidationRules.MaxTrainingLogLength)
		{
			throw new ArgumentException(ErrorMessages.TrainingLogIsLong, nameof(trainingLog));
		}
		
		var training = new Training
		{
			UserId = userId,
			TrainingDate = trainingDate,
			TrainingDuration = duration,
			TrainingLog = trainingLog,
			Attempts = attempts,
		};
		
		await _trainingRepository.AddAsync(training);
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

	public async Task<ICollection<Training>> GetTrainingsByUserIdAsync(int userId)
	{
		return await _trainingRepository.GetAllByUserIdAsync(userId);
	}
	
	public async Task<ICollection<Training>> GetTrainingsByPeriodAsync(int userId, DateOnly start, DateOnly end)
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
		return await _trainingRepository.GetTrainingsByPeriodAsync(userId, start, end);
	}

	public async Task EditTrainingLogAsync(int id, string text)
	{
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

	public async Task<ICollection<Attempt>> GetAllAttemptsByTrainingIdAsync(int trainingId)
	{
		var training = await _trainingRepository.GetByIdAsync(trainingId);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={trainingId})");
		}
		
		return await _trainingRepository.GetAllAttemptsByTrainingIdAsync(trainingId);
	}

	public async Task<ICollection<Attempt>> GetTopAttemptsByTrainingIdAsync(int trainingId, int amount)
	{
		var training = await _trainingRepository.GetByIdAsync(trainingId);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={trainingId})");
		}

		return await _trainingRepository.GetTopAttemptsByTrainingIdAsync(trainingId, amount);
	}

	public async Task<ICollection<Discipline>> GetAllDisciplinesByTrainingIdAsync(int trainingId)
	{
		var training = await _trainingRepository.GetByIdAsync(trainingId);

		if (training == null)
		{
			throw new KeyNotFoundException(ErrorMessages.TrainingNotFound + $" (id={trainingId})");
		}

		return await _trainingRepository.GetAllDisciplinesByTrainingIdAsync(trainingId);
	}
}
