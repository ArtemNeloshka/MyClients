// TODO: change to AttemptService
// using Moq;
// using MyClients.BLL.Services;
// using MyClients.DAL.Entities;
// using MyClients.DAL.Repositories;
//
// namespace MyClients.BLL.Tests;
//
// public class PersonalRecordServiceTests
// {
// 	// add record sad path
// 	[Fact]
// 	public async Task AddRecordAsync_InvalidDate_ThrowsArgumentException()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
// 		var service = new PersonalRecordService(mockRepo.Object);
//
// 		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
// 			service.AddRecordAsync(
// 				user: new User(),
// 				grade: new Grade(),
// 				date: DateOnly.FromDateTime(DateTime.Now).AddDays(1),
// 				discipline: new Discipline()));
//
// 		Assert.Contains("Date of the record cannot be bigger than today's.", exception.Message);
// 	}
// 	
// 	// add record happy path
// 	[Fact]
// 	public async Task AddRecordAsync_ValidData_AddsRecordToDatabase()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
//
// 		var recordToAdd = new PersonalRecord()
// 		{
// 			User = new User() {Name = "Test"},
// 			Grade = new Grade(),
// 			Discipline = new Discipline(),
// 			RecordDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
// 		};
// 		
// 		var service = new PersonalRecordService(mockRepo.Object);
//
// 		await service.AddRecordAsync(
// 			user: recordToAdd.User,
// 			grade: recordToAdd.Grade,
// 			discipline: recordToAdd.Discipline,
// 			date: recordToAdd.RecordDate);
// 		
// 		mockRepo.Verify(r => r.AddAsync(It.Is<PersonalRecord>(pr =>
// 			pr.RecordDate == recordToAdd.RecordDate &&
// 			pr.User.Name == recordToAdd.User.Name)), Times.Once);
// 	}
// 	
// 	// update record sad path
// 	[Fact]
// 	public async Task UpdateRecordAsync_InvalidDate_ThrowsArgumentException()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
// 		var service = new PersonalRecordService(mockRepo.Object);
//
// 		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
// 			service.UpdateRecordAsync(
// 				id: 1,
// 				grade: null,
// 				date: DateOnly.FromDateTime(DateTime.Now).AddDays(1)));
//
// 		Assert.Contains("Date of the record cannot be bigger than today's.", exception.Message);
// 	}
// 	
// 	// update record sad path
// 	[Fact]
// 	public async Task UpdateRecordAsync_PersonalRecordDoesntExist_ThrowsKeyNotFoundException()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
// 		var service = new PersonalRecordService(mockRepo.Object);
// 		
// 		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
// 			service.UpdateRecordAsync(
// 				id: -1,
// 				grade: null,
// 				date: DateOnly.FromDateTime(DateTime.Now).AddDays(-1)));
//
// 		Assert.Contains("There's no personal record to update.", exception.Message);
// 	}
// 	
// 	// update record happy path
// 	[Fact]
// 	public async Task UpdateRecordAsync_ValidGrade_UpdatesRecordInDatabase()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
//
// 		var existingRecord = new PersonalRecord()
// 		{
// 			Id = 1,
// 			Grade = new Grade() {Name = "old grade"},
// 		};
// 		
// 		mockRepo.Setup(r => r.GetByIdAsync(existingRecord.Id))
// 			.ReturnsAsync(existingRecord);
// 		
// 		var service = new PersonalRecordService(mockRepo.Object);
//
// 		await service.UpdateRecordAsync(
// 			id: existingRecord.Id,
// 			grade: new Grade() {Name = "new grade"},
// 			date: null);
// 		
// 		Assert.Equal("new grade", existingRecord.Grade.Name);
// 		mockRepo.Verify(r => r.UpdateAsync(existingRecord), Times.Once);
// 	}
// 	
// 	// update record happy path
// 	[Fact]
// 	public async Task UpdateRecordAsync_ValidDate_UpdatesRecordInDatabase()
// 	{
// 		var mockRepo = new Mock<IPersonalRecordRepository>();
//
// 		var existingRecord = new PersonalRecord()
// 		{
// 			Id = 1,
// 			RecordDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
// 		};
// 		
// 		mockRepo.Setup(r => r.GetByIdAsync(existingRecord.Id))
// 			.ReturnsAsync(existingRecord);
// 		
// 		var service = new PersonalRecordService(mockRepo.Object);
//
// 		var newDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-10);
// 		
// 		await service.UpdateRecordAsync(
// 			id: existingRecord.Id,
// 			grade: null,
// 			date: newDate);
// 		
// 		Assert.Equal(newDate, existingRecord.RecordDate);
// 		mockRepo.Verify(r => r.UpdateAsync(existingRecord), Times.Once);
// 	}
// }