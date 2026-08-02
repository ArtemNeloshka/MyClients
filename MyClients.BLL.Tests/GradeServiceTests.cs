using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Tests;

public class GradeServiceTests
{
	private readonly Mock<IGradeRepository> _mockRepo;
	private readonly IGradeService _service;

	public GradeServiceTests()
	{
		_mockRepo = new Mock<IGradeRepository>();
		_service = new GradeService(_mockRepo.Object);
	}
	
	// get by id sad path
	[Fact]
	public async Task GetGradeByIdAsync_GradeNotFound_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Grade?)null);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_service.GetGradeByIdAsync(invalidId));

		Assert.Contains(ErrorMessages.GradeNotFound, exception.Message);
		
		_mockRepo.Verify(r => r.GetByIdAsync(invalidId), Times.Once);
	}
	
	// get by id happy path
	[Fact]
	public async Task GetGradeByIdAsync_ValidData_ReturnsGrade()
	{
		var grade = new Grade { Id = 1 };
		
		_mockRepo.Setup(r => r.GetByIdAsync(grade.Id))
			.ReturnsAsync(grade);

		var result = await _service.GetGradeByIdAsync(grade.Id);

		Assert.Equal(grade.Id, result.Id);
		
		_mockRepo.Verify(r => r.GetByIdAsync(grade.Id), Times.Once);
	}
	
	// get all grades happy path
	// get by id happy path
	[Fact]
	public async Task GetAllGradesAsync_ValidData_ReturnsGradesList()
	{
		var grade = new Grade { Id = 1 };
		
		_mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync([grade]);

		var result = await _service.GetAllGradesAsync();

		Assert.Equal(1, result.Count);
		Assert.Contains(grade, result);
		
		_mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
}
