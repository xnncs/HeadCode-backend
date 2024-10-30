namespace HeadCode.Api.Endpoints.Problems.Add;

using MediatR;

public class AddProblemRequest : IRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
}