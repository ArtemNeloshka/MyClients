namespace MyClients.DAL.Entities;

public class Grade
{
	// PK
	public int Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public int Value { get; set; }
	
	// Navigation properties
	public ICollection<PersonalRecord> PersonalRecords { get; set; } = new List<PersonalRecord>();
}