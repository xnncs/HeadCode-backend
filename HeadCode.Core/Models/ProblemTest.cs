namespace HeadCode.Core.Models;

public class ProblemTest
{
    public Guid Id { get; set; }
    
    public Problem Problem { get; set; }
    public Guid ProblemId { get; set; }
    
    public DateTime DateCreated { get; set; }
}