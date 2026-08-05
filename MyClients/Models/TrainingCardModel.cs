namespace MyClients.Models;

public class TrainingCardModel
{
	public int TrainingId { get; set; }
	public string TrainingDate { get; set; } = String.Empty;
	public string TrainingDuration { get; set; } = String.Empty;
	public string DisciplinesTrained { get; set; } = String.Empty;
	public string BestGradesClimbed { get; set; } = String.Empty;
	public string ClimbingGym { get; set; } = "Climbing Gym";
}
