namespace HeadCode.Core.Models;

public class Problem
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public List<DateTime> DatesUpdated { get; set; } = [];

    public List<ProblemTest> Tests { get; set; } = [];

    public List<User> Solvers { get; set; } = [];

    public static Problem Create(string title, string description)
    {
        return new Problem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            DateCreated = DateTime.UtcNow
        };
    }
}