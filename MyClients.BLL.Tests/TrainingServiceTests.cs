using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Tests;

public class TrainingServiceTests
{
	private readonly Mock<ITrainingRepository> _mockRepo;
	private readonly ITrainingService _service;

	public TrainingServiceTests()
	{
		_mockRepo = new Mock<ITrainingRepository>();
		_service = new TrainingService(_mockRepo.Object);
	}
	
	// create training sad path
	// invalid date
	[Fact]
	public async Task CreateTrainingAsync_InvalidDate_ThrowsArgumentException()
	{
		var userId = 1;
		var trainingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
		var duration = new TimeSpan(hours: 1, minutes: 1, seconds: 1);
		var trainingLog = string.Empty;
		var attempts = new List<Attempt> { new Attempt { } };
		
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.CreateTrainingAsync(userId, trainingDate, duration, trainingLog, attempts));

		Assert.Contains(ErrorMessages.InvalidDateHigherThanToday, exception.Message);
	}
	
	// invalid duration
	[Theory]
	[InlineData(-1, 1, 1)]
	[InlineData(0, 0, 0)]
	public async Task CreateTrainingAsync_InvalidDuration_ThrowsArgumentException(int hours, int minutes, int seconds)
	{
		var userId = 1;
		var trainingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
		var duration = new TimeSpan(hours, minutes, seconds);
		var trainingLog = string.Empty;
		var attempts = new List<Attempt> { new Attempt { } };
	
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.CreateTrainingAsync(userId, trainingDate, duration, trainingLog, attempts));

		Assert.Contains(ErrorMessages.InvalidDuration, exception.Message);
	}
	
	// invalid log
	[Fact]
	public async Task CreateTrainingAsync_InvalidLog_ThrowsArgumentException()
	{
		var userId = 1;
		var trainingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
		var duration = new TimeSpan(hours: 1, minutes: 1, seconds: 1);
		var trainingLog = new string('A', ValidationRules.MaxTrainingLogLength + 1);
		var attempts = new List<Attempt> { new Attempt { } };
		
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.CreateTrainingAsync(userId, trainingDate, duration, trainingLog, attempts));

		Assert.Contains(ErrorMessages.TrainingLogIsLong, exception.Message);
	}
	
	// create training happy path
	[Fact]
	public async Task CreateTrainingAsync_ValidData_UpdatesDB()
	{
		var userId = 1;
		var trainingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
		var duration = new TimeSpan(hours: 1, minutes: 1, seconds: 1);
		var trainingLog = new string('A', ValidationRules.MaxTrainingLogLength - 1);
		var attempts = new List<Attempt> { new Attempt { } };

		var training = new Training
		{
			UserId = userId,
			TrainingDate = trainingDate,
			TrainingDuration = duration,
			TrainingLog = trainingLog,
			Attempts = attempts,
		};

		await _service.CreateTrainingAsync(userId, trainingDate, duration, trainingLog, attempts);
		
		_mockRepo.Verify(r => r.AddAsync(It.Is<Training>(t =>
			t.UserId == userId &&
			t.TrainingDate == trainingDate &&
			t.TrainingDuration == duration &&
			t.TrainingLog == trainingLog &&
			t.Attempts == attempts)), Times.Once);
	}

	// get training by id sad path
	[Fact]
	public async Task GetTrainingByIdAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.GetTrainingByIdAsync(invalidId));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// get training by id happy path
	[Fact]
	public async Task GetTrainingByIdAsync_ValidData_ReturnsTraining()
	{
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Show this training."
		};

		_mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		var training = await _service.GetTrainingByIdAsync(1);

		Assert.NotNull(training);
		Assert.Contains("Show this training.", training.TrainingLog);

		_mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
	}
	
	// get training by user id happy path
	[Fact]
	public async Task GetTrainingsByUserIdAsync_ValidData_ReturnsTrainingsList()
	{
		var user = new User { Id = 1 };
		var training1 = new Training { UserId = user.Id };
		var training2 = new Training { UserId = user.Id };

		_mockRepo.Setup(r => r.GetAllByUserIdAsync(user.Id))
			.ReturnsAsync([training1, training2]);

		var result = await _service.GetTrainingsByUserIdAsync(user.Id);
		
		Assert.Equal(2, result.Count);
		Assert.Contains(training1, result);
		Assert.Contains(training2, result);
		
		_mockRepo.Verify(r => r.GetAllByUserIdAsync(user.Id), Times.Once);
	}
	
	// get by period sad path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_InvalidEndDate_ThrowsArgumentException()
	{
		var userId = 1;
		var startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
		var endDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
		
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.GetTrainingsByPeriodAsync(userId, startDate, endDate));

		Assert.Contains(ErrorMessages.InvalidDateHigherThanToday, exception.Message);
	}
	
	// get by period sad path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_StartDateBiggerThanEndDate_ThrowsArgumentException()
	{
		var userId = 1;
		var startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
		var endDate = startDate.AddDays(-1);
		
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.GetTrainingsByPeriodAsync(userId, startDate, endDate));

		Assert.Contains(ErrorMessages.DateStartHigherThanEnd, exception.Message);
	}
	
	// get by period happy path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_ValidData_ReturnsTrainingsList()
	{
		var existingValidTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Valid training.",
			TrainingDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
		};
		
		var existingInvalidTraining = new Training()
		{
			Id = 2,
			TrainingLog = "Invalid training.",
			TrainingDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-10),
		};

		int userId = 1;
		DateOnly startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-2);
		DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);

		_mockRepo.Setup(r => r.GetTrainingsByPeriodAsync(userId, startDate, endDate))
			.ReturnsAsync([existingValidTraining]);

		var resultTrainings = await _service.GetTrainingsByPeriodAsync(userId, startDate, endDate);

		Assert.Single(resultTrainings);
		Assert.Equal("Valid training.", resultTrainings.First().TrainingLog);
		
		_mockRepo.Verify(r => r.GetTrainingsByPeriodAsync(userId, startDate, endDate), Times.Once);
	}
	
	// edit log sad path
	// log is too long
	[Fact]
	public async Task EditTrainingLogAsync_InvalidLog_ThrowsArgumentException()
	{
		var training = new Training { Id = 1, TrainingLog = "TestLog" };
		var newLog = new string('A', ValidationRules.MaxTrainingLogLength + 1);
		
		_mockRepo.Setup(r => r.GetByIdAsync(training.Id))
			.ReturnsAsync(training);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.EditTrainingLogAsync(training.Id, newLog));

		Assert.Contains(ErrorMessages.TrainingLogIsLong, exception.Message);
	}

	// training not found
	[Fact]
	public async Task EditTrainingLogAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.EditTrainingLogAsync(invalidId, "Test"));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// edit log happy path
	[Fact]
	public async Task EditTrainingLogAsync_ValidData_EditsTrainingLog()
	{
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Start log."
		};

		_mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		await _service.EditTrainingLogAsync(1, "Edited log.");

		Assert.Contains("Edited log.", existingTraining.TrainingLog);
		Assert.DoesNotContain("Start log.", existingTraining.TrainingLog);
		
		_mockRepo.Verify(r => r.UpdateAsync(existingTraining), Times.Once);
	}
	
	// delete training sad path
	[Fact]
	public async Task DeleteTrainingAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.DeleteTrainingAsync(invalidId));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// delete training happy path
	[Fact]
	public async Task DeleteTrainingAsync_ValidData_DeletesTraining()
	{
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Delete this training."
		};

		_mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		await _service.DeleteTrainingAsync(1);

		_mockRepo.Verify(r => r.DeleteAsync(existingTraining), Times.Once);
	}
	
	// get all attempts sad path
	[Fact]
	public async Task GetAllAttemptsByTrainingIdAsync_TrainingNotFound_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.GetAllAttemptsByTrainingIdAsync(invalidId));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// get all attempts happy path
	[Fact]
	public async Task GetAllAttemptsByTrainingIdAsync_ValidData_ReturnsAttemptsList()
	{
		var training1Id = 1;
		var training2Id = 2;

		var training1 = new Training { Id = training1Id };
		var training2 = new Training { Id = training2Id };
		
		var attempt1Training1 = new Attempt { TrainingId = training1Id, };
		var attempt2Training1 = new Attempt { TrainingId = training1Id, };
		var attempt1Training2 = new Attempt { TrainingId = training2Id, };

		_mockRepo.Setup(r => r.GetByIdAsync(training1.Id))
			.ReturnsAsync(training1);
		
		_mockRepo.Setup(r => r.GetAllAttemptsByTrainingIdAsync(1))
			.ReturnsAsync([attempt1Training1, attempt2Training1]);
		
		var result = await _service.GetAllAttemptsByTrainingIdAsync(1);

		Assert.Equal(2, result.Count);
		foreach (var a in result)
		{ 
			Assert.Equal(1, a.TrainingId);
		}
		
		_mockRepo.Verify(r => r.GetAllAttemptsByTrainingIdAsync(1), Times.Once);
	}
	
	// get top attempts sad path
	[Fact]
	public async Task GetTopAttemptsByTrainingIdAsync_TrainingNotFound_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.GetTopAttemptsByTrainingIdAsync(invalidId, 3));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// get all attempts happy path
	[Fact]
	public async Task GetTopAttemptsByTrainingIdAsync_ValidData_ReturnsAttemptsList()
	{
		var training1Id = 1;
		var training2Id = 2;
		var attemptLowestGradeValue = 2;
		var attemptHighestGradeValue = attemptLowestGradeValue + 1;
		var attemptWrongTrainingGradeValue = attemptLowestGradeValue + 2;
		var amount = 1;

		var training1 = new Training { Id = training1Id };
		var training2 = new Training { Id = training2Id };
		
		var attempt1Training1 = new Attempt
		{
			TrainingId = training1Id, 
			Grade = new Grade { Value = attemptLowestGradeValue }
		};
		var attempt2Training1 = new Attempt
		{
			TrainingId = training1Id,
			Grade = new Grade { Value = attemptHighestGradeValue }
		};
		var attempt1Training2 = new Attempt
		{
			TrainingId = training2Id, 
			Grade = new Grade { Value = attemptWrongTrainingGradeValue }
		};

		_mockRepo.Setup(r => r.GetByIdAsync(training1.Id))
			.ReturnsAsync(training1);

		_mockRepo.Setup(r => r.GetTopAttemptsByTrainingIdAsync(training1Id, amount))
			.ReturnsAsync([attempt2Training1]);

		var result = await _service.GetTopAttemptsByTrainingIdAsync(training1Id, amount);

		Assert.Equal(1, result.Count);
		foreach (var a in result)
		{ 
			Assert.Equal(training1Id, a.TrainingId);
			Assert.Equal(attemptHighestGradeValue, a.Grade.Value);
		}
		
		_mockRepo.Verify(r => r.GetTopAttemptsByTrainingIdAsync(training1Id, amount), Times.Once);
	}
	
	// get all disciplines sad path
	[Fact]
	public async Task GetAllDisciplinesByTrainingIdAsync_TrainingNotFound_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Training?)null);
		
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			_service.GetAllDisciplinesByTrainingIdAsync(invalidId));

		Assert.Contains(ErrorMessages.TrainingNotFound, exception.Message);
	}
	
	// get all disciplines happy path
	[Fact]
	public async Task GetAllDisciplinesByTrainingIdAsync_ValidData_ReturnsAttemptsList()
	{
		var boulderingDiscipline = new Discipline { Name = Disciplines.Bouldering };
		var topRopeDiscipline = new Discipline { Name = Disciplines.TopRopeClimbing };
		var leadClimbingDiscipline = new Discipline { Name = Disciplines.LeadClimbing };

		var training1 = new Training { Id = 1 };
		var training2 = new Training { Id = 2 };
			
		var attempt1Training1 = new Attempt
		{
			TrainingId = training1.Id, 
			Discipline = boulderingDiscipline,
		};
		var attempt2Training1 = new Attempt
		{
			TrainingId = training1.Id, 
			Discipline = topRopeDiscipline
		};
		var attempt3Training1 = new Attempt
		{
			TrainingId = training1.Id, 
			Discipline = topRopeDiscipline
		};
		var attempt1Training2 = new Attempt
		{
			TrainingId = training2.Id, 
			Discipline = leadClimbingDiscipline,
		};

		_mockRepo.Setup(r => r.GetByIdAsync(training1.Id))
			.ReturnsAsync(training1);

		_mockRepo.Setup(r => r.GetAllDisciplinesByTrainingIdAsync(training1.Id))
			.ReturnsAsync([boulderingDiscipline, topRopeDiscipline]);

		var result = await _service.GetAllDisciplinesByTrainingIdAsync(training1.Id);

		Assert.NotNull(result);
		Assert.Equal(2, result.Count);
		Assert.Contains(result, d => d.Name == Disciplines.Bouldering);
		Assert.Contains(result, d => d.Name == Disciplines.TopRopeClimbing);
		
		_mockRepo.Verify(r => r.GetAllDisciplinesByTrainingIdAsync(training1.Id), Times.Once);
	}
}
