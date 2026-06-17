namespace MyClients.DAL.Entities;

public class User
{
	// PK
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Surname { get; set; } = string.Empty;
	public DateOnly Birthday { get; set; }
	public string Email { get; set; } = string.Empty;
	
	// Navigation properties
	public ICollection<Training> Trainings { get; set; } = new List<Training>();
	public ICollection<PersonalRecord> PersonalRecords { get; set; } = new List<PersonalRecord>();
}