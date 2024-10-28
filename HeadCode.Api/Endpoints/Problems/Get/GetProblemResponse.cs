namespace HeadCode.Api.Endpoints.Problems.GetAllProblems;

using Core.Models;

public class GetProblemResponse
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public List<DateTime> DatesUpdated { get; set; } = [];
}