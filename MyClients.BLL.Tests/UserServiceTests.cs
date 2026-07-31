using Moq;
using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.BLL.Services;
using MyClients.Domain.Entities;
using static MyClients.Domain.Constants.ErrorMessages;

namespace MyClients.BLL.Tests;

public class UserServiceTests
{
	private readonly Mock<IUserRepository> _mockRepo;
	private readonly IUserService _service;

	public UserServiceTests()
	{
		_mockRepo = new Mock<IUserRepository>();
		_service = new UserService(_mockRepo.Object);
	}
	
	// edit info sad path
	[Fact]
	public async Task EditUserInfoAsync_BirthdayBiggerThanToday_ThrowsArgumentException()
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.EditUserInfoAsync(
				id: 1,
				name: "TestName",
				surname: "TestSurname",
				birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(1),
				favouriteDisciplineId: null));

		Assert.Contains(InvalidDateHigherThanToday, exception.Message);
	}
	
	// edit info sad path
	[Fact]
	public async Task EditUserInfoAsync_UserNotFound_ThrowsKeyNotFoundException()
	{
		var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_service.EditUserInfoAsync(
				id: -1,
				name: "TestName",
				surname: "TestSurname",
				birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				favouriteDisciplineId: null));

		Assert.Contains(UserNotFound, exception.Message);
	}

	// edit info happy path
	// name
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesName()
	{
		var userNameToEdit = new User()
		{
			Id = 1,
			Name = "OldName",
			Surname = "Surname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};

		_mockRepo.Setup(r => r.GetByIdAsync(1))
			.ReturnsAsync(userNameToEdit);

		await _service.EditUserInfoAsync(
			id: 1,
			name: "NewName", 
			surname: null, 
			birthday: null,
			favouriteDisciplineId: null);

		Assert.Equal("NewName", userNameToEdit.Name);
		
		_mockRepo.Verify(r => r.UpdateAsync(userNameToEdit), Times.Once);
	}
	
	// surname
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesSurname()
	{
		var userSurnameToEdit = new User()
		{
			Id = 2,
			Name = "Name",
			Surname = "OldSurname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};

		_mockRepo.Setup(r => r.GetByIdAsync(2))
			.ReturnsAsync(userSurnameToEdit);
		
		await _service.EditUserInfoAsync(
			id: 2,
			name: null, 
			surname: "NewSurname", 
			birthday: null,
			favouriteDisciplineId: null);

		Assert.Equal("NewSurname", userSurnameToEdit.Surname);
		
		_mockRepo.Verify(r => r.UpdateAsync(userSurnameToEdit), Times.Once);
	}
	
	// birthday
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesBirthday()
	{
		var userDateToEdit = new User()
		{
			Id = 3,
			Name = "Name",
			Surname = "Surname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};
		
		_mockRepo.Setup(r => r.GetByIdAsync(3))
			.ReturnsAsync(userDateToEdit);
		
		await _service.EditUserInfoAsync(
			id: 3,
			name: null, 
			surname: null, 
			birthday: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
			favouriteDisciplineId: null);

		Assert.Equal(DateOnly.FromDateTime(DateTime.Now).AddDays(-1), userDateToEdit.Birthday);
		
		_mockRepo.Verify(r => r.UpdateAsync(userDateToEdit), Times.Once);
	}
	
	// favourite discipline
	[Fact]
	public async Task EditUserInfoAsync_ValidData_UpdatesFavouriteDiscipline()
	{
		var user = new User
		{
			Id = 4,
			Name = "Name",
			Surname = "Surname",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
		};

		_mockRepo.Setup(r => r.GetByIdAsync(4))
			.ReturnsAsync(user);

		await _service.EditUserInfoAsync(
			id: 4,
			name: null,
			surname: null,
			birthday: null,
			favouriteDisciplineId: 1);
		
		Assert.Equal(1, user.FavouriteDisciplineId);
		
		_mockRepo.Verify(r => r.UpdateAsync(user), Times.Once);
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
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.GetUserByEmailAsync(invalidEmail));

		Assert.Contains(InvalidEmail, exception.Message);
	}

	// get by email happy path
	[Fact]
	public async Task GetUserByEmailAsync_ValidEmail_ReturnsUserWithValidEmail()
	{
		var existingUser = new User()
		{
			Id = 1,
			Email = "test@gmail.com",
		};

		_mockRepo.Setup(r => r.GetByEmailAsync("test@gmail.com"))
			.ReturnsAsync(existingUser);

		var userByEmail = await _service.GetUserByEmailAsync(existingUser.Email);
		
		Assert.NotNull(userByEmail);
		Assert.Equal(existingUser.Email, userByEmail.Email);
		
		_mockRepo.Verify(r => r.GetByEmailAsync("test@gmail.com"), Times.Once);
	}
	
	// register sad path
	// null name
	[Fact]
	public async Task RegisterUserAsync_InvalidFirstName_ThrowsArgumentException()
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.RegisterUserAsync(
				firstName: string.Empty,
				lastName: "Test",
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains(NameIsNullOrEmpty, exception.Message);
	}
	
	// null surname
	[Fact]
	public async Task RegisterUserAsync_InvalidLastName_ThrowsArgumentException()
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.RegisterUserAsync(
				firstName: "Test",
				lastName: string.Empty,
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains(SurnameIsNullOrEmpty, exception.Message);
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
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.RegisterUserAsync(
				firstName: "Test",
				lastName: "Test",
				email: invalidEmail,
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: "TestPassword"));

		Assert.Contains(InvalidEmail, exception.Message);
	}
	
	// invalid birthday
	[Fact]
	public async Task RegisterUserAsync_InvalidBirthday_ThrowsArgumentException()
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.RegisterUserAsync(
				firstName: "Test",
				lastName: "Test",
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(1),
				password: "TestPassword"));

		Assert.Contains(InvalidDateHigherThanToday, exception.Message);
	}
	
	// invalid password
	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("short")]
	public async Task RegisterUserAsync_InvalidPassword_ThrowsArgumentException(string invalidPassword)
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.RegisterUserAsync(
				firstName: "TestName",
				lastName: "TestSurname",
				email: "test@gmail.com",
				birthdate: DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
				password: invalidPassword));

		Assert.Contains(PasswordIsShort, exception.Message);
	}
	
	// register happy path
	[Fact]
	public async Task RegisterUserAsync_ValidData_AddsNewUserToRepository()
	{
		var userToRegister = new User()
		{
			Name = "Name",
			Surname = "Surname",
			Email = "test@gmail.com",
			Birthday = DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
		};

		await _service.RegisterUserAsync(
			firstName: userToRegister.Name,
			lastName: userToRegister.Surname,
			email: userToRegister.Email,
			birthdate: userToRegister.Birthday,
			password: "TestPassword");
		
		_mockRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
			u.Name == userToRegister.Name &&
			u.Surname == userToRegister.Surname &&
			u.Email == userToRegister.Email &&
			u.Birthday == userToRegister.Birthday
		)), Times.Once);
	}
	
	// login sad path
	// invalid email
	[Theory]
	[InlineData("")]
	[InlineData("invalidDomain@test.com")]
	[InlineData("noAtgmail.com")]
	[InlineData("      ")]
	[InlineData("shortDomain@gmail.c")]
	[InlineData("@gmail.com")]
	public async Task LoginUserAsync_InvalidEmail_ThrowsArgumentException(string invalidEmail)
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.LoginUserAsync(invalidEmail, "TestPassword"));

		Assert.Contains(InvalidEmail, exception.Message);
	}
	
	// invalid password
	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("short")]
	public async Task LoginUserAsync_InvalidPassword_ThrowsArgumentException(string invalidPassword)
	{
		var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
			_service.LoginUserAsync(
				email: "test@gmail.com",
				password: invalidPassword));

		Assert.Contains(PasswordIsShort, exception.Message);
	}
	
	// email not found
	[Fact]
	public async Task LoginUserAsync_EmailDoesntFound_ThrowsArgumentException()
	{
		_mockRepo.Setup(r => r.GetByEmailAsync("non_existence_email@gmail.com"))
			.ReturnsAsync((User?)null);

		var result = await _service.LoginUserAsync(
			email: "non_existence_email@gmail.com", 
			password: "ValidPassword");
		
		Assert.False(result.Success);
		Assert.Contains(UserNotFound, result.ErrorMessage);
	}
	
	// incorrect password
	[Fact]
	public async Task LoginUserAsync_PasswordsDoesntMatch_ThrowsArgumentException()
	{
		var user = new User
		{
			Name = "TestName",
			Surname = "TestSurname",
			Email = "test@gmail.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
		};

		_mockRepo.Setup(r => r.GetByEmailAsync("test@gmail.com"))
			.ReturnsAsync(user);

		var result = await _service.LoginUserAsync(
			email: "test@gmail.com", 
			password: "IncorrectPassword");
		
		Assert.False(result.Success);
		Assert.Equal(PasswordIncorrect, result.ErrorMessage);
	}
	
	// password hash is null
	[Fact]
	public async Task LoginUserAsync_PasswordHashIsNull_ThrowsArgumentException()
	{
		var user = new User
		{
			Name = "TestName",
			Surname = "TestSurname",
			Email = "test@gmail.com",
			PasswordHash = null!,
		};

		_mockRepo.Setup(r => r.GetByEmailAsync("test@gmail.com"))
			.ReturnsAsync(user);

		var result = await _service.LoginUserAsync(
			email: "test@gmail.com", 
			password: "SomePassword");
		
		Assert.False(result.Success);
		Assert.Equal(PasswordIncorrect, result.ErrorMessage);
	}
	
	// login happy path
	[Fact]
	public async Task LoginUserAsync_FoundUserByEmail_ReturnsTrue()
	{
		var plainPassword = "TestPassword";
		var existingUser = new User()
		{
			Id = 1,
			Name = "Test",
			Email = "test@gmail.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
		};

		_mockRepo.Setup(r => r.GetByEmailAsync("test@gmail.com"))
			.ReturnsAsync(existingUser);
		
		var result = await _service.LoginUserAsync(existingUser.Email, plainPassword);

		Assert.True(result.Success);
		
		_mockRepo.Verify(r => r.GetByEmailAsync("test@gmail.com"), Times.Once);
	}
	
	// login happy path
	[Fact]
	public async Task LoginUserAsync_NoExistingEmail_ReturnsFalse()
	{
		var plainPassword = "TestPassword";
		var existingUser = new User()
		{
			Id = 1,
			Name = "Test",
			Email = "test@gmail.com",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
		};

		_mockRepo.Setup(r => r.GetByEmailAsync("test@gmail.com"))
			.ReturnsAsync(existingUser);

		var result = await _service.LoginUserAsync("invalid" + existingUser.Email, plainPassword);

		Assert.False(result.Success);
		
		_mockRepo.Verify(r => r.GetByEmailAsync("invalidtest@gmail.com"), Times.Once);
	}
}
