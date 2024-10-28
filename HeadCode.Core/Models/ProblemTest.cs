namespace HeadCode.Core.Models;

public class ProblemTest
{
    public Guid Id { get; set; }
    
    public Problem Problem { get; set; }
    public Guid ProblemId { get; set; }
    
    public string InputData { get; set; }
    public string CorrectOutputData { get; set; }
    
    public DateTime DateCreated { get; set; }
    public List<DateTime> DatesUpdated { get; set; } = [];

    public static ProblemTest Create(Guid problemId, string inputData, string correctOutputData)
    {
        return new ProblemTest
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            InputData = inputData,
            CorrectOutputData = correctOutputData,
            ProblemId = problemId
        };
    }
}