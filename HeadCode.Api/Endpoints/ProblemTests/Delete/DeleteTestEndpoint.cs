namespace HeadCode.Api.Endpoints.ProblemTests.Delete;

using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class DeleteTestEndpoint : Endpoint<DeleteTestRequest, Results<Ok, NotFound, InternalServerError>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<DeleteTestEndpoint> _logger;

    public DeleteTestEndpoint(ILogger<DeleteTestEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Delete("api/tests/delete/{Id}");
    }

    public override async Task<Results<Ok, NotFound, InternalServerError>> ExecuteAsync(
        DeleteTestRequest request, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            int count = await _dbContext.ProblemTests.Where(x => x.Id == request.Id)
                                        .ExecuteDeleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            if (count == 0) return TypedResults.NotFound();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete ProblemTest {Id}", request.Id);
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok();
    }
}