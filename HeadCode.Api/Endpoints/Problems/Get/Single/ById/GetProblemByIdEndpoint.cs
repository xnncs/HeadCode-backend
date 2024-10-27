namespace HeadCode.Api.Endpoints.Problems.Get.Single.ById;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using GetAllProblems;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class GetProblemByIdEndpoint : Endpoint<GetProblemByIdRequest, Results<Ok<GetProblemResponse>, NotFound>>
{
    public GetProblemByIdEndpoint(ILogger<GetProblemByIdEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    private readonly ILogger<GetProblemByIdEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;

    public override void Configure()
    {
        Get("api/problems/get/{Id}");
        AllowAnonymous();
    }
    
    public override async Task<Results<Ok<GetProblemResponse>, NotFound>> HandleAsync(GetProblemByIdRequest request, CancellationToken cancellationToken)
    {
        Problem? problem = await _dbContext.Problems.AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (problem == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(problem.Adapt<GetProblemResponse>());
    }
}