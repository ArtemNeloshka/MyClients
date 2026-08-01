using MyClients.BLL.Interfaces.Repositories;
using MyClients.BLL.Interfaces.Services;
using MyClients.Domain.Constants;
using MyClients.Domain.Entities;

namespace MyClients.BLL.Services;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		this._userRepository = userRepository;
	}
	
	public async Task EditUserInfoAsync(int id, string? name, string? surname, DateOnly? birthday, int? favouriteDisciplineId)
	{
		// validate input
		if (birthday != null && birthday > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday, nameof(birthday));
		}
		
		// getting user from DB
		var user = await _userRepository.GetByIdAsync(id);

		if (user == null)
		{
			throw new KeyNotFoundException(ErrorMessages.UserNotFound + $" (id={id})");
		}
		
		// editing info
		if (!string.IsNullOrWhiteSpace(name))
			user.Name = name;
		
		if (!string.IsNullOrWhiteSpace(surname))
			user.Surname = surname;
		
		if (birthday != null)
			user.Birthday = (DateOnly)birthday;

		if (favouriteDisciplineId != null)
			user.FavouriteDisciplineId = favouriteDisciplineId;
		
		// saving changes
		await _userRepository.UpdateAsync(user);
	}

	public async Task<ICollection<User>> GetAllUsersAsync()
	{
		return (await _userRepository.GetAllAsync()).ToList();
	}

	public async Task<User?> GetUserByEmailAsync(string email)
	{
		// validate input
		if (!ValidationRules.IsValidEmail(email))
		{
			throw new ArgumentException(ErrorMessages.InvalidEmail, nameof(email));
		}

		return await _userRepository.GetByEmailAsync(email);
	}

	public async Task RegisterUserAsync(string firstName, string lastName, string email, DateOnly birthdate, string password)
	{
		// validate input
		if (string.IsNullOrWhiteSpace(firstName))
		{
			throw new ArgumentException(ErrorMessages.NameIsNullOrEmpty);
		}
		
		if (string.IsNullOrWhiteSpace(lastName))
		{
			throw new ArgumentException(ErrorMessages.SurnameIsNullOrEmpty);
		}

		if (!ValidationRules.IsValidEmail(email))
		{
			throw new ArgumentException(ErrorMessages.InvalidEmail);
		}

		if (birthdate > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException(ErrorMessages.InvalidDateHigherThanToday);
		}

		if (string.IsNullOrWhiteSpace(password) || password.Length < ValidationRules.MinPasswordLength)
		{
			throw new ArgumentException(ErrorMessages.PasswordIsShort, nameof(password));
		}

		if (await GetUserByEmailAsync(email) != null)
		{
			throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);
		}
		
		// initialisation of a new user
		var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
		
		var newUser = new User()
		{
			Name = firstName,
			Surname = lastName,
			Email = email,
			Birthday = birthdate,
			PasswordHash = passwordHash,
		};
		
		await _userRepository.AddAsync(newUser);
	}

	public async Task<(bool Success, string? ErrorMessage)> LoginUserAsync(string email, string password)
	{
		// validate input
		if (!ValidationRules.IsValidEmail(email))
		{
			throw new ArgumentException(ErrorMessages.InvalidEmail, nameof(email));
		}

		if (string.IsNullOrWhiteSpace(password) || password.Length < ValidationRules.MinPasswordLength)
		{
			throw new ArgumentException(ErrorMessages.PasswordIsShort, nameof(password));
		}
		
		// getting user by email
		var user = await GetUserByEmailAsync(email);

		if (user == null)
		{
			return (false, ErrorMessages.UserNotFound);
		}

		if (string.IsNullOrWhiteSpace(user.PasswordHash))
		{
			return (false, ErrorMessages.PasswordIncorrect);
		}

		bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
		
		return isPasswordMatch ? (true, null) : (false, ErrorMessages.PasswordIncorrect);
	}
	
	public async Task<Grade?> GetBestGradeInDisciplineAsync(int userId, int disciplineId)
	{
		return await _userRepository.GetBestGradeInDisciplineAsync(userId, disciplineId);
	}
}
