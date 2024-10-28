namespace HeadCode.Api.Endpoints.ProblemTests.Get.All;

using DataAccess.DatabaseContexts;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class GetAllTestsEndpoint : EndpointWithoutRequest<List<GetTestResponse>>
{
    private readonly ILogger<GetAllTestsEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;

    public GetAllTestsEndpoint(ILogger<GetAllTestsEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("api/tests/get");
        AllowAnonymous();
    }

    public override async Task<List<GetTestResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ProblemTests.AsNoTracking()
                               .ProjectToType<GetTestResponse>()
                               .ToListAsync(cancellationToken);
    }
}