using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Services;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Tests;

public class TrainingServiceTests
{
	// add log sad path
	[Fact]
	public async Task AddTrainingLogAsync_EmptyText_ThrowsArgumentException()
	{
		// arrange
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);
		
		// act
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.AddTrainingLogAsync(1, "   "));

		Assert.Contains("Log cannot be empty", exception.Message);
	}

	// add log sad path
	[Fact]
	public async Task AddTrainingLogAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		// arrange
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);
		
		// act
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			service.AddTrainingLogAsync(-1, "test1"));

		Assert.Contains("Training id=-1 is not found.", exception.Message);
	}
	
	// add log happy path
	[Fact]
	public async Task AddTrainingLogAsync_ValidData_UpdatesTrainingLog()
	{
		// arrange
		var mockRepo = new Mock<ITrainingRepository>();

		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Training began.",
		};
		
		// learning
		mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);
		
		var service = new TrainingService(mockRepo.Object);
		
		// act
		await service.AddTrainingLogAsync(1, "Training ended.");

		Assert.Contains("Training ended.", existingTraining.TrainingLog);
		
		mockRepo.Verify(r => r.UpdateAsync(existingTraining), Times.Once);
	}
	
	// edit log sad path
	[Fact]
	public async Task EditTrainingLogAsync_EmptyText_ThrowsArgumentException()
	{
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.EditTrainingLogAsync(1, string.Empty));

		Assert.Contains("Log cannot be empty.", exception.Message);
	}
	
	// edit log sad path
	[Fact]
	public async Task EditTrainingLogAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			service.EditTrainingLogAsync(-1, "Test"));

		Assert.Contains("Training id=-1 is not found.", exception.Message);
	}
	
	// edit log happy path
	[Fact]
	public async Task EditTrainingLogAsync_ValidData_EditsTrainingLog()
	{
		var mockRepo = new Mock<ITrainingRepository>();
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Start log."
		};

		mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		var service = new TrainingService(mockRepo.Object);

		await service.EditTrainingLogAsync(1, "Edited log.");

		Assert.Contains("Edited log.", existingTraining.TrainingLog);
		Assert.DoesNotContain("Start log.", existingTraining.TrainingLog);
		
		mockRepo.Verify(r => r.UpdateAsync(existingTraining), Times.Once);
	}
	
	// delete training sad path
	[Fact]
	public async Task DeleteTrainingAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			service.DeleteTrainingAsync(-1));

		Assert.Contains("Training id=-1 is not found.", exception.Message);
	}
	
	// delete training happy path
	[Fact]
	public async Task DeleteTrainingAsync_ValidData_DeletesTraining()
	{
		var mockRepo = new Mock<ITrainingRepository>();
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Delete this training."
		};

		mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		var service = new TrainingService(mockRepo.Object);

		await service.DeleteTrainingAsync(1);

		mockRepo.Verify(r => r.DeleteAsync(existingTraining), Times.Once);
	}

	// get training by id sad path
	[Fact]
	public async Task GetTrainingByIdAsync_NullTraining_ThrowsKeyNotFoundException()
	{
		var mockRepo = new Mock<ITrainingRepository>();

		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
			service.GetTrainingByIdAsync(-1));

		Assert.Contains("Training id=-1 is not found.", exception.Message);
	}
	
	// get training by id happy path
	[Fact]
	public async Task GetTrainingByIdAsync_ValidData_ReturnsTraining()
	{
		var mockRepo = new Mock<ITrainingRepository>();
		var existingTraining = new Training()
		{
			Id = 1,
			TrainingLog = "Show this training."
		};

		mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(existingTraining);

		var service = new TrainingService(mockRepo.Object);

		var training = await service.GetTrainingByIdAsync(1);

		Assert.NotNull(training);
		Assert.Contains("Show this training.", training.TrainingLog);

		mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
	}
	
	// get by period sad path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_InvalidEndDate_ThrowsArgumentException()
	{
		var mockRepo = new Mock<ITrainingRepository>();
		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.GetTrainingsByPeriodAsync(
				start: DateOnly.FromDateTime(DateTime.Now).AddDays(-1), 
				end: DateOnly.FromDateTime(DateTime.Now).AddDays(1)));

		Assert.Contains("Date cannot be higher than today.", exception.Message);
	}
	
	// get by period sad path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_StartDateBiggerThanEndDate_ThrowsArgumentException()
	{
		var mockRepo = new Mock<ITrainingRepository>();
		var service = new TrainingService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.GetTrainingsByPeriodAsync(
				start: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				end: DateOnly.FromDateTime(DateTime.Now).AddDays(-2)));

		Assert.Contains("Start date cannot be higher than an end date.", exception.Message);
	}
	
	// get by period happy path
	[Fact]
	public async Task GetTrainingsByPeriodAsync_ValidData_ReturnsTrainingsInGivenPeriod()
	{
		var mockRepo = new Mock<ITrainingRepository>();

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

		mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync(new List<Training>{existingValidTraining, existingInvalidTraining});

		var service = new TrainingService(mockRepo.Object);

		var resultTrainings = await service.GetTrainingsByPeriodAsync(
			start: DateOnly.FromDateTime(DateTime.Now).AddDays(-2),
			end: DateOnly.FromDateTime(DateTime.Now));

		Assert.Single(resultTrainings);
		Assert.Equal("Valid training.", resultTrainings.First().TrainingLog);
		
		mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
}