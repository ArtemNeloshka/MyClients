using MyClients.DAL.Entities;

namespace MyClients.BLL.Interfaces;

public interface IUserService
{
	Task EditUserInfoAsync(int id, string? name, string? surname, DateOnly? birthday);
	Task<ICollection<User>> GetAllUsersAsync();
	Task<User?> GetUserByEmailAsync(string email);
	Task RegisterUserAsync(string firstName, string lastName, string email, DateOnly birthdate, string password);
	Task<bool> LoginUserAsync(string email, string password);
}