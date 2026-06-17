namespace MyClients.DAL.Entities;

public class PersonalRecord
{
	// PK
	public int Id { get; set; }
	public int UserId { get; set; }
	public int DisciplineId { get; set; }
	public int GradeId { get; set; }
	public DateOnly RecordDate { get; set; }
	
	// Navigation properties
	public User User { get; set; } = null!;
	public Discipline Discipline { get; set; } = null!;
	public Grade Grade { get; set; } = null!;
}