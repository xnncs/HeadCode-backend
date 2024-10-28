namespace HeadCode.Api.Endpoints.ProblemTests.Get;

public class GetTestResponse
{
    public Guid Id { get; set; }

    public Guid ProblemId { get; set; }

    public string InputData { get; set; }
    public string CorrectOutputData { get; set; }

    public DateTime DateCreated { get; set; }
    public List<DateTime> DatesUpdated { get; set; } = [];
}