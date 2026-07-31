namespace MyClients.Domain.Entities;

public class ClimbResult
{
	public int Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public string? Description { get; set; }

	public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
}
