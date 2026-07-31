namespace MyClients.Domain.Entities;

public class Discipline
{
	// PK
	public int Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public string Description { get; set; } = string.Empty;
	
	// Navigation properties
	public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
	public ICollection<User> UsersAsFavouriteDiscipline { get; set; } = new List<User>();
}