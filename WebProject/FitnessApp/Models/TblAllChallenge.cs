namespace FitnessApp.Models;

public partial class TblAllChallenge
{
    
    public int Id { get; set; }
    
    public string? Title {get; set;}

    public string? Category { get; set; }

    public string? DifficultyLevel { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Instruction { get; set; }

}