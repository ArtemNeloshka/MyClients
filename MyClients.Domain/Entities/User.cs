namespace MyClients.Domain.Entities;

public class User
{
	// PK
	public int Id { get; set; }
	public int? FavouriteDisciplineId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Surname { get; set; } = string.Empty;
	public DateOnly Birthday { get; set; }
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	
	// Navigation properties
	public Discipline? FavouriteDiscipline { get; set; }
	public ICollection<Training> Trainings { get; set; } = new List<Training>();
}
