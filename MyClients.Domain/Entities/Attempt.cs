namespace MyClients.Domain.Entities;

public class Attempt
{
	public int Id { get; set; }
	public int TrainingId { get; set; }
	public int DisciplineId { get; set; }
	public int GradeId { get; set; }
	public int ClimbResultId { get; set; }
	public TimeSpan Timestamp { get; set; }

	public Training Training { get; set; } = null!;
	public Discipline Discipline { get; set; } = null!;
	public Grade Grade { get; set; } = null!;
	public ClimbResult ClimbResult { get; set; } = null!;
}
