namespace HeadCode.Api.Endpoints.ProblemTests.Get.ByProblemId;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class GetTestsByProblemIdEndpoint : Endpoint<GetTestsByProblemIdRequest,
    Results<Ok<List<GetTestResponse>>, BadRequest<string>>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetTestsByProblemIdEndpoint> _logger;

    public GetTestsByProblemIdEndpoint(ILogger<GetTestsByProblemIdEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("api/tests/get/byProblemId/{Id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<List<GetTestResponse>>, BadRequest<string>>> ExecuteAsync(
        GetTestsByProblemIdRequest request, CancellationToken cancellationToken)
    {
        Problem? problem = await _dbContext.Problems.AsNoTracking()
                                           .Include(x => x.Tests)
                                           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (problem == null) return TypedResults.BadRequest("Problem not found");

        return TypedResults.Ok(problem.Tests
                                      .AsQueryable()
                                      .ProjectToType<GetTestResponse>()
                                      .ToList());
    }
}