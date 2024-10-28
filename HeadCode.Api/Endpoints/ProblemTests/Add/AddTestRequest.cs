namespace HeadCode.Api.Endpoints.ProblemTests.Add;

public class AddTestRequest
{
    public Guid ProblemId { get; set; }
    
    public string InputData { get; set; }
    public string CorrectOutputData { get; set; }
}