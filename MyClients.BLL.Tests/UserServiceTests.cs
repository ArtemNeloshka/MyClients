using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Services;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Tests;

public class UserServiceTests
{
	// edit info sad path
	[Fact]
	public async Task EditUserInfoAsync_BirthdayBiggerThanToday_ThrowsArgumentException()
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.EditUserInfoAsync(
				id: 1,
				name: "TestName",
				surname: "TestSurname",
				birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(1)));

		Assert.Contains("Birthdate cannot be higher than today", exception.Message);
	}
	
	// edit info sad path
	[Fact]
	public async Task EditUserInfoAsync_UserNotFound_ThrowsKeyNotFoundException()
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			service.EditUserInfoAsync(
				id: -1,
				name: "TestName",
				surname: "TestSurname",
				birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(-1)));

		Assert.Contains("User id=-1 is not found.", exception.Message);
	}

	// edit info happy path
	// name
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesName()
	{
		var mockRepo = new Mock<IUserRepository>();

		var userNameToEdit = new User()
		{
			Id = 1,
			Name = "OldName",
			Surname = "Surname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};

		mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(userNameToEdit);
		
		var service = new UserService(mockRepo.Object);

		await service.EditUserInfoAsync(
			id: 1,
			name: "NewName", 
			surname: null, 
			birthday: null);

		Assert.Equal("NewName", userNameToEdit.Name);
		
		mockRepo.Verify(r => r.UpdateAsync(userNameToEdit), Times.Once);
	}
	
	// surname
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesSurname()
	{
		var mockRepo = new Mock<IUserRepository>();
		
		var userSurnameToEdit = new User()
		{
			Id = 2,
			Name = "Name",
			Surname = "OldSurname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};

		mockRepo.Setup(r => r.GetByIdAsync(2))
			.ReturnsAsync(userSurnameToEdit);
		
		var service = new UserService(mockRepo.Object);
		
		await service.EditUserInfoAsync(
			id: 2,
			name: null, 
			surname: "NewSurname", 
			birthday: null);

		Assert.Equal("NewSurname", userSurnameToEdit.Surname);
		
		mockRepo.Verify(r => r.UpdateAsync(userSurnameToEdit), Times.Once);
	}
	
	// birthday
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesBirthday()
	{
		var mockRepo = new Mock<IUserRepository>();
		
		var userDateToEdit = new User()
		{
			Id = 3,
			Name = "Name",
			Surname = "Surname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};
		
		mockRepo.Setup(r => r.GetByIdAsync(3))
			.ReturnsAsync(userDateToEdit);
		
		var service = new UserService(mockRepo.Object);

		
		await service.EditUserInfoAsync(
			id: 3,
			name: null, 
			surname: null, 
			birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(-1));

		Assert.Equal(DateOnly.FromDateTime(DateTime.Now).AddDays(-1), userDateToEdit.Birthday);
		
		mockRepo.Verify(r => r.UpdateAsync(userDateToEdit), Times.Once);
	}
	
	// get by email sad path
	[Theory]
	[InlineData("")]
	[InlineData("invalidDomain@test.com")]
	[InlineData("noAtgmail.com")]
	[InlineData("      ")]
	[InlineData("shortDomain@gmail.c")]
	[InlineData("@gmail.com")]
	[InlineData("invalidEmail@gmail.com@gmail.com")]
	[InlineData("invalidEm@il@gmail.com")]
	[InlineData(" @gmail.com")]
	[InlineData("with space@gmail.com")]
	public async Task GetUserByEmailAsync_InvalidEmail_ThrowsArgumentException(string invalidEmail)
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.GetUserByEmailAsync(invalidEmail));

		Assert.Contains("Email is not valid.", exception.Message);
	}

	// get by email happy path
	[Fact]
	public async Task GetUserByEmailAsync_ValidEmail_ReturnsUserWithValidEmail()
	{
		var mockRepo = new Mock<IUserRepository>();

		var existingUser = new User()
		{
			Id = 1,
			Email = "Test@gmail.com",
		};

		mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync(new List<User>{existingUser});
		
		var service = new UserService(mockRepo.Object);

		var userByEmail = await service.GetUserByEmailAsync(existingUser.Email);
		
		Assert.NotNull(userByEmail);
		Assert.Equal(existingUser.Email, userByEmail.Email);
		
		mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
	
	// register sad path
	// null name
	[Fact]
	public async Task RegisterUserAsync_InvalidFirstName_ThrowsArgumentException()
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.RegisterUserAsync(
				firstName: string.Empty,
				lastName: "Test",
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains("Name or surname cannot be null.", exception.Message);
	}
	
	// null surname
	[Fact]
	public async Task RegisterUserAsync_InvalidLastName_ThrowsArgumentException()
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.RegisterUserAsync(
				firstName: "Test",
				lastName: string.Empty,
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains("Name or surname cannot be null.", exception.Message);
	}
	
	// invalid email
	[Theory]
	[InlineData("")]
	[InlineData("invalidDomain@test.com")]
	[InlineData("noAtgmail.com")]
	[InlineData("      ")]
	[InlineData("shortDomain@gmail.c")]
	[InlineData("@gmail.com")]
	public async Task RegisterUserAsync_InvalidEmail_ThrowsArgumentException(string invalidEmail)
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.RegisterUserAsync(
				firstName: "Test",
				lastName: "Test",
				email: invalidEmail,
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains("Email doesn't match the pattern.", exception.Message);
	}
	
	// invalid birthday
	[Fact]
	public async Task RegisterUserAsync_InvalidBirthday_ThrowsArgumentException()
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.RegisterUserAsync(
				firstName: "Test",
				lastName: "Test",
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(1),
				password: "TestPassword"));

		Assert.Contains("Birthdate cannot be higher than today.", exception.Message);
	}
	
	// TODO: password test
	
	// register happy path
	[Fact]
	public async Task RegisterUserAsync_ValidData_AddsNewUserToRepository()
	{
		var mockRepo = new Mock<IUserRepository>();

		var userToRegister = new User()
		{
			Name = "Name",
			Surname = "Surname",
			Email = "test@gmail.com",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
		};
		
		var service = new UserService(mockRepo.Object);

		await service.RegisterUserAsync(
			firstName: userToRegister.Name,
			lastName: userToRegister.Surname,
			email: userToRegister.Email,
			birthdate: userToRegister.Birthday,
			password: "TestPassword");
		
		mockRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
			u.Name == userToRegister.Name &&
			u.Surname == userToRegister.Surname &&
			u.Email == userToRegister.Email &&
			u.Birthday == userToRegister.Birthday
		)), Times.Once);
	}
	
	// login sad path
	[Theory]
	[InlineData("")]
	[InlineData("invalidDomain@test.com")]
	[InlineData("noAtgmail.com")]
	[InlineData("      ")]
	[InlineData("shortDomain@gmail.c")]
	[InlineData("@gmail.com")]
	public async Task LoginUserAsync_InvalidEmail_ThrowsArgumentException(string invalidEmail)
	{
		var mockRepo = new Mock<IUserRepository>();
		var service = new UserService(mockRepo.Object);

		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			service.LoginUserAsync(invalidEmail, "TestPassword"));

		Assert.Contains("Email is not valid.", exception.Message);
	}
	
	// login happy path
	[Fact]
	public async Task LoginUserAsync_FoundUserByEmail_ReturnsTrue()
	{
		var mockRepo = new Mock<IUserRepository>();

		var plainPassword = "TestPassword";
		var existingUser = new User()
		{
			Id = 1,
			Name = "Test",
			Email = "test@gmail.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
		};

		mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync(new List<User>{existingUser});
		
		var service = new UserService(mockRepo.Object);

		var result = await service.LoginUserAsync(existingUser.Email, plainPassword);

		Assert.True(result.Success);
		
		mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
	
	// login happy path
	[Fact]
	public async Task LoginUserAsync_NoExistingEmail_ReturnsFalse()
	{
		var mockRepo = new Mock<IUserRepository>();

		var plainPassword = "TestPassword";
		var existingUser = new User()
		{
			Id = 1,
			Name = "Test",
			Email = "test@gmail.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
		};

		mockRepo.Setup(r => r.GetAllAsync())
			.ReturnsAsync(new List<User>{existingUser});
		
		var service = new UserService(mockRepo.Object);

		var result = await service.LoginUserAsync("invalid" + existingUser.Email, plainPassword);

		Assert.False(result.Success);
		
		mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
	}
}