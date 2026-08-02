using MyClients.Domain.Entities;

namespace MyClients.BLL.Interfaces.Services;

public interface IUserService
{
	Task RegisterUserAsync(string firstName, string lastName, string email, DateOnly birthdate, string password);
	Task<ICollection<User>> GetAllUsersAsync();
	Task<User?> GetUserByEmailAsync(string email);
	Task EditUserInfoAsync(int id, string? name, string? surname, DateOnly? birthday, int? favouriteDisciplineId);
	Task<(bool Success, string? ErrorMessage)> LoginUserAsync(string email, string password);
	Task<Grade?> GetBestGradeInDisciplineAsync(int userId, int disciplineId);
}