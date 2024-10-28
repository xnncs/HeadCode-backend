namespace HeadCode.Api.Endpoints.Problems.GetAllProblems;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class GetAllProblemsEndpoint : EndpointWithoutRequest<List<GetProblemResponse>>
{
    private readonly ILogger<GetAllProblemsEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;

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