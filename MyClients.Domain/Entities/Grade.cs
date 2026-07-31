namespace MyClients.Domain.Entities;

public class Grade
{
	// PK
	public int Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public int Value { get; set; }
	
	// Navigation properties
	public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
}