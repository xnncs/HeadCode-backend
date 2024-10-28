namespace HeadCode.Api.Endpoints.ProblemTests.Add;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class AddTestEndpoint : Endpoint<AddTestRequest, Results<Created, BadRequest<string>, InternalServerError>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<AddTestEndpoint> _logger;

    public AddTestEndpoint(ILogger<AddTestEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("api/tests/add");
    }

    public override async Task<Results<Created, BadRequest<string>, InternalServerError>> ExecuteAsync(
        AddTestRequest request, CancellationToken cancellationToken)
    {
        bool problemExists = await _dbContext.Problems.AnyAsync(x => x.Id == request.ProblemId, cancellationToken);
        if (!problemExists)
        {
            _logger.LogInformation("Problem with id: {ProblemId} does not exist", request.ProblemId);
            return TypedResults.BadRequest($"Problem with id: {request.ProblemId} does not exist");
        }

        ProblemTest problemTest = ProblemTest.Create(request.ProblemId, request.InputData, request.CorrectOutputData);

        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _dbContext.ProblemTests.Add(problemTest);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while adding a ProblemTest");
            return TypedResults.InternalServerError();
        }

        return TypedResults.Created();
    }
}