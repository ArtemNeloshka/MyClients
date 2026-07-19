using MyClients.BLL.Interfaces;
using MyClients.Constants;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;

namespace MyClients.BLL.Services;

public class PersonalRecordService : Service, IPersonalRecordService
{
	private readonly IPersonalRecordRepository _personalRecordRepository;

	public PersonalRecordService(IPersonalRecordRepository personalRecordRepository)
	{
		this._personalRecordRepository = personalRecordRepository;
	}
	
	public async Task<PersonalRecord?> GetRecordByIdAsync(int id)
	{
		return await _personalRecordRepository.GetByIdAsync(id);
	}

	public async Task<ICollection<PersonalRecord>> GetRecordsByUserIdAsync(int userId)
	{
		// getting records from DB
		var records = await _personalRecordRepository
			.GetRecordsByUserIdAsync(userId);

		return records.ToList();
	}

	public async Task AddRecordAsync(User user, Grade grade, DateOnly date, Discipline discipline)
	{
		// validating input
		if (date > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday, nameof(date));
		}
		
		// creating new object
		var newRecord = new PersonalRecord
		{
			User = user,
			Grade = grade,
			RecordDate = date,
			Discipline = discipline,
		};
		
		// adding to db
		await _personalRecordRepository.AddAsync(newRecord);
	}

	public async Task UpdateRecordAsync(int id, Grade? grade, DateOnly? date)
	{
		// validate input
		if (date > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday, nameof(date));
		}
		
		// getting the record from DB
		var record = await GetRecordByIdAsync(id);

		if (record == null)
		{
			throw new KeyNotFoundException(ErrorMessages.PersonalRecordNotFound + $" (id={id})");
		}

		if (grade != null)
			record.Grade = grade;
		if (date != null)
			record.RecordDate = (DateOnly)date;

		await _personalRecordRepository.UpdateAsync(record);
	}
}