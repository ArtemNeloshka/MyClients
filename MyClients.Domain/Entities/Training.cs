namespace MyClients.Domain.Entities;

public class Training
{
	// PK
	public int Id { get; set; }
	public int UserId { get; set; }
	public DateOnly TrainingDate { get; set; }
	public string TrainingLog { get; set; } = String.Empty;
	
	// Navigation property
	public User User { get; set; } = null!;
}