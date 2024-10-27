namespace HeadCode.Api.Endpoints.Problems.Update;

public class UpdateProblemRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}