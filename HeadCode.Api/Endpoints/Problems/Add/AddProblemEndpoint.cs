namespace HeadCode.Api.Endpoints.Problems.Add;

using FastEndpoints;
using HeadCode.Core.Models;
using HeadCode.DataAccess.DatabaseContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Storage;

public class AddProblemEndpoint : Endpoint<AddProblemRequest, Created>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<AddProblemEndpoint> _logger;

    public AddProblemEndpoint(ILogger<AddProblemEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    public override void Configure()
    {
        Post("api/problems/add");
    }

    public override async Task<Created> ExecuteAsync(
        AddProblemRequest request, CancellationToken cancellationToken)
    {
        Problem created = Problem.Create(request.Title, request.Description);

        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _dbContext.Problems.Add(created);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }

        return TypedResults.Created();
    }
}