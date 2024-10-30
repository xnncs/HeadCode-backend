namespace HeadCode.Api.Endpoints.ProblemSolving.Solve;

public class SolveProblemRequest
{
    public Guid ProblemId { get; set; }
    public string Code { get; set; }
    public string Language { get; set; }
}