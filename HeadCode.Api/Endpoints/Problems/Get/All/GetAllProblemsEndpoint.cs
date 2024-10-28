namespace HeadCode.Api.Endpoints.Problems.Get.All;

using FastEndpoints;
using HeadCode.DataAccess.DatabaseContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class GetAllProblemsEndpoint : EndpointWithoutRequest<List<GetProblemResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAllProblemsEndpoint> _logger;

    public GetAllProblemsEndpoint(ILogger<GetAllProblemsEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("api/problems/get");
        AllowAnonymous();
    }

    public override async Task<List<GetProblemResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Problems.AsNoTracking()
                               .ProjectToType<GetProblemResponse>()
                               .ToListAsync(cancellationToken);
    }
}