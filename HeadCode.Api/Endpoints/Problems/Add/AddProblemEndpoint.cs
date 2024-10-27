namespace HeadCode.Api.Endpoints.Problems.AddProblem;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Storage;

public class AddProblemEndpoint : Endpoint<AddProblemRequest, Results<Created, BadRequest<string>, InternalServerError>>
{
    public AddProblemEndpoint(ILogger<AddProblemEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    private readonly ILogger<AddProblemEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;
    

    public override void Configure()
    {
        Post("api/problems/add");
    }

    public override async Task<Results<Created, BadRequest<string>, InternalServerError>> HandleAsync(AddProblemRequest request, CancellationToken cancellationToken)
    {
        Problem created = Problem.Create(request.Title, request.Description);
        
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _dbContext.Problems.Add(created);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return TypedResults.InternalServerError();
        }
        
        return TypedResults.Created();
    }
}