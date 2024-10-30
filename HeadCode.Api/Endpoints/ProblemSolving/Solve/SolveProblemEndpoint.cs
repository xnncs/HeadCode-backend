namespace HeadCode.Api.Endpoints.ProblemSolving.Solve;

using Core.Enums;
using Core.KafkaContracts;
using Core.KafkaContracts.HelpingModels;
using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class SolveProblemEndpoint : Endpoint<SolveProblemRequest, Results<Ok, BadRequest<string>, InternalServerError>>
{
    public SolveProblemEndpoint(ILogger<SolveProblemEndpoint> logger, ApplicationDbContext dbContext, IProducerAccessor accessor)
    {
        _logger = logger;
        _dbContext = dbContext;
        _producer = accessor.GetProducer(ProducerName);
    }
    
    private readonly ILogger<SolveProblemEndpoint> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMessageProducer _producer;
    
    private const string ProducerName = "headcode-api-tests-solving-topic";
    

    public override void Configure()
    {
        Post("api/problems/solve");
    }

    public override async Task<Results<Ok, BadRequest<string>, InternalServerError>> ExecuteAsync(SolveProblemRequest request, CancellationToken cancellationToken)
    {
        Languages? language = SwitchLanguage(request.Language);
        if (language == null)
        {
            return TypedResults.BadRequest<string>("Invalid language");
        }
        
        Problem? problem = await _dbContext.Problems.Include(x => x.Tests)
                                           .FirstOrDefaultAsync(x => x.Id == request.ProblemId, cancellationToken);
        if (problem == null)
        {
            return TypedResults.BadRequest<string>("Problem not found");
        }
        
        try
        {
            await _producer.ProduceAsync(ProducerName, RunTestsMessage.Create(language.Value, request.Code,
                    problem.Tests.Select(t => TestInfo.Create(t.InputData, t.CorrectOutputData)).ToList())
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Kafka failed to produce problem");
            return TypedResults.InternalServerError();
        }
        _logger.LogInformation("Kafka produced problem");
        
        return TypedResults.Ok();
    }

    private Languages? SwitchLanguage(string language)
    {
        return language switch
        {
            "csharp" => Languages.CSharp,
            "python" => Languages.Python,
            _        => null
        };
    }
}