namespace SolvingMicroservice.Api.Kafka.Handlers;

using System.Text.Json;
using HeadCode.Core.KafkaContracts;
using KafkaFlow;

public class RunTestsMessageHandler : IMessageHandler<RunTestsMessage>
{
    private readonly ILogger<RunTestsMessageHandler> _logger;

    public RunTestsMessageHandler(ILogger<RunTestsMessageHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, RunTestsMessage message)
    {
        _logger.LogInformation($"RunTestMessage received: {JsonSerializer.Serialize(message)}");
    }
}