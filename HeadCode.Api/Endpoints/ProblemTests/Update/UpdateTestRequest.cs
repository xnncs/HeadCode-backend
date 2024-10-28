namespace HeadCode.Api.Endpoints.ProblemTests.Update;

public class UpdateTestRequest
{
    public Guid Id { get; set; }

    public string InputData { get; set; }
    public string CorrectOutputData { get; set; }
}