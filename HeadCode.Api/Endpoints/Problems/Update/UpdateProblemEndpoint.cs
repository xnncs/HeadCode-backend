namespace HeadCode.Api.Endpoints.Problems.Update;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class UpdateProblemEndpoint : Endpoint<UpdateProblemRequest, Results<Ok, BadRequest<string>, NotFound, InternalServerError>>
{
    public UpdateProblemEndpoint(ILogger<UpdateProblemEndpoint> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    private readonly ILogger<UpdateProblemEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;
    
    
    public override void Configure()
    {
        Put("problems/{Id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, BadRequest<string>, NotFound, InternalServerError>> HandleAsync(UpdateProblemRequest request, CancellationToken cancellationToken)
    {
        Problem? problem = await _dbContext.Problems
                                       .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (problem == null)
        {
            return TypedResults.NotFound();
        }
        
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            problem.Title = request.Title;
            problem.Description = request.Description;
            problem.DatesUpdated.Add(DateTime.UtcNow);
        
            await SendNoContentAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok();
    }
}