namespace HeadCode.Api.Endpoints.Problems.Delete;

using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class DeleteProblemEndpoint : Endpoint<DeleteProblemRequest, Results<Ok, NotFound, InternalServerError>>
{
    public DeleteProblemEndpoint(ILogger<DeleteProblemEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    private readonly ILogger<DeleteProblemEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;
    
    public override void Configure()
    {
        Delete("problems/{Id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound, InternalServerError>> HandleAsync(DeleteProblemRequest request, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            int count = await _dbContext.Problems.Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            if (count == 0)
            {
                return TypedResults.NotFound();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete problem {Id}", request.Id);
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok();
    }
}
