namespace HeadCode.Core.HelpingModels;

using Enums;
using Models;

public class Solution
{
    public Guid Id { get; set; }
    
    public User User { get; set; }
    public Guid UserId { get; set; }
    
    public Problem Problem { get; set; }
    public Guid ProblemId { get; set; }
    
    public SolutionResult SolutionResult { get; set; }
}