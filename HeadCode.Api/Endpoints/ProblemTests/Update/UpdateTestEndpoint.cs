namespace HeadCode.Api.Endpoints.ProblemTests.Update;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class
    UpdateTestEndpoint : Endpoint<UpdateTestRequest, Results<Ok, BadRequest<string>, NotFound, InternalServerError>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateTestEndpoint> _logger;

    public UpdateTestEndpoint(ILogger<UpdateTestEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Put("api/tests/update");
    }

    public override async Task<Results<Ok, BadRequest<string>, NotFound, InternalServerError>> ExecuteAsync(
        UpdateTestRequest request, CancellationToken cancellationToken)
    {
        ProblemTest? problemTest = await _dbContext.ProblemTests
                                                   .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (problemTest == null) return TypedResults.NotFound();

        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            problemTest.DatesUpdated.Add(DateTime.UtcNow);
            problemTest.InputData = request.InputData;
            problemTest.CorrectOutputData = request.CorrectOutputData;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to update ProblemTest {Id}", request.Id);
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok();
    }
}