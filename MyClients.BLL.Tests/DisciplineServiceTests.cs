using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Tests;

public class DisciplineServiceTests
{
	private readonly Mock<IDisciplineRepository> _mockRepo;
	private readonly IDisciplineService _service;

	public DisciplineServiceTests()
	{
		_mockRepo = new Mock<IDisciplineRepository>();
		_service = new DisciplineService(_mockRepo.Object);
	}
	
	// get by id sad path
	[Fact]
	public async Task GetDisciplineByIdAsync_DisciplineNotFound_ThrowsKeyNotFoundException()
	{
		var invalidId = -1;
		
		_mockRepo.Setup(r => r.GetByIdAsync(invalidId))
			.ReturnsAsync((Discipline?)null);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_service.GetDisciplineByIdAsync(invalidId));

		Assert.Contains(ErrorMessages.DisciplineNotFoundMessage, exception.Message);
		
		_mockRepo.Verify(r => r.GetByIdAsync(invalidId), Times.Once);
	}
	
	// get by id happy path
	[Fact]
	public async Task GetDisciplineByIdAsync_ValidData_ReturnsDiscipline()
	{
		var discipline = new Discipline { Id = 1 };
		
		_mockRepo.Setup(r => r.GetByIdAsync(discipline.Id))
			.ReturnsAsync(discipline);

		var result = await _service.GetDisciplineByIdAsync(discipline.Id);

		Assert.Equal(discipline.Id, result.Id);
		
		_mockRepo.Verify(r => r.GetByIdAsync(discipline.Id), Times.Once);
	}
	
	// get all happy path
	[Fact]
	public async Task GetAllDisciplinesAsync_ValidData_ReturnsDisciplinesList()
	{
		var discipline = new Discipline { Id = 1 };
		
		_mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync([discipline]);

		var result = await _service.GetAllDisciplinesAsync();

		Assert.Equal(1, result.Count);
		Assert.Contains(discipline, result);
		
		_mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
	
	// get by name sad path
	[Fact]
	public async Task GetDisciplineByNameAsync_DisciplineNotFound_ThrowsKeyNotFoundException()
	{
		var invalidName = "Test";
		
		_mockRepo.Setup(r => r.GetDisciplineByNameAsync(invalidName))
			.ReturnsAsync((Discipline?)null);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_service.GetDisciplineByNameAsync(invalidName));

		Assert.Contains(ErrorMessages.DisciplineNotFoundMessage, exception.Message);
		
		_mockRepo.Verify(r => r.GetDisciplineByNameAsync(invalidName), Times.Once);
	}
	
	// get by name happy path
	[Fact]
	public async Task GetDisciplineByNameAsync_ValidData_ReturnsDiscipline()
	{
		var discipline = new Discipline { Name = Disciplines.Bouldering };
		
		_mockRepo.Setup(r => r.GetDisciplineByNameAsync(discipline.Name))
			.ReturnsAsync(discipline);

		var result = await _service.GetDisciplineByNameAsync(discipline.Name);

		Assert.Equal(discipline.Name, result.Name);
		
		_mockRepo.Verify(r => r.GetDisciplineByNameAsync(discipline.Name), Times.Once);
	}
}
