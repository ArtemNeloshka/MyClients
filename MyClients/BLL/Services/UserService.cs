using System.Text.RegularExpressions;
using MyClients.BLL.Interfaces;
using MyClients.DAL.Entities;
using MyClients.DAL.Repositories;

namespace MyClients.BLL.Services;

public class UserService : Service, IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		this._userRepository = userRepository;
	}
	
	public async Task EditUserInfoAsync(int id, string? name, string? surname, DateOnly? birthday)
	{
		// validate input
		if (birthday != null && birthday > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException("Birthdate cannot be higher than today", nameof(birthday));
		}
		
		// getting user from DB
		var user = await _userRepository.GetByIdAsync(id);

		if (user == null)
		{
			throw new KeyNotFoundException($"User id={id} is not found.");
		}
		
		// editing info
		if (!string.IsNullOrWhiteSpace(name))
			user.Name = name;
		
		if (!string.IsNullOrWhiteSpace(surname))
			user.Surname = surname;
		
		if (birthday != null)
			user.Birthday = (DateOnly)birthday;
		
		// saving changes
		await _userRepository.UpdateAsync(user);
	}

	public async Task<ICollection<User>> GetAllUsersAsync()
	{
		var allUsers = (await _userRepository.GetAllAsync()).ToList();

		return allUsers;
	}

	public async Task<User?> GetUserByEmailAsync(string email)
	{
		// validate input
		if (!IsValidEmail(email))
		{
			throw new ArgumentException("Email is not valid.", nameof(email));
		}
		
		// getting all users
		var users = await _userRepository.GetAllAsync();

		return users.FirstOrDefault(u => u.Email == email);
	}

	public async Task RegisterUserAsync(string firstName, string lastName, string email, DateOnly birthdate)
	{
		// validate input
		if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
		{
			throw new ArgumentException("Name or surname cannot be null.");
		}

		if (!IsValidEmail(email))
		{
			throw new ArgumentException("Email doesn't match the pattern.");
		}

		if (birthdate > DateOnly.FromDateTime(DateTime.Now))
		{
			throw new ArgumentException("Birthdate cannot be higher than today.");
		}
		
		// initialisation of a new user
		var newUser = new User()
		{
			Name = firstName,
			Surname = lastName,
			Email = email,
			Birthday = birthdate,
		};
		
		await _userRepository.AddAsync(newUser);
	}

	public async Task<bool> LoginUserAsync(string email)
	{
		// validate input
		if (!IsValidEmail(email))
		{
			throw new ArgumentException("Email is not valid.", nameof(email));
		}
		
		// getting user by email
		var user = await GetUserByEmailAsync(email);

		return user != null;
	}

	private static bool IsValidEmail(string email)
	{
		var emailPattern = @"[\w-]+@gmail\.com";

		return Regex.IsMatch(email, emailPattern);
	}
}