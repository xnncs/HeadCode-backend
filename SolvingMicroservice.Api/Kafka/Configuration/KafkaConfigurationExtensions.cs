namespace SolvingMicroservice.Api.Kafka.Configuration;

using Handlers;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;

public static class KafkaConfigurationExtensions
{
    public static IServiceCollection AddConfiguredKafka(this IServiceCollection services)
    {
        const string topicName = "tests-solving-topic";
        const string consumerServer = "kafka";
        const string consumerPort = "9092";
        
        const string groupId = "tests-solving-group";
        
        services.AddKafka(kafka => kafka
                                  .AddCluster(cluster => cluster
                                                        .WithBrokers(new[] { $"{consumerServer}:{consumerPort}" })
                                                        .CreateTopicIfNotExists(topicName, 1, 1)
                                                        .AddConsumer(consumer => consumer
                                                            .Topic(topicName)
                                                            .WithGroupId(groupId)
                                                            .WithBufferSize(100)
                                                            .WithWorkersCount(10)
                                                            .AddMiddlewares(middlewares => middlewares
                                                                .AddDeserializer<JsonCoreDeserializer>()
                                                                .AddTypedHandlers(h => h.AddHandler<RunTestsMessageHandler>())
                                                             )
                                                         )
                                   )
        );
        
        return services;
    }

    public static WebApplication UseKafka(this WebApplication app)
    {
        IKafkaBus? kafkaBus = app.Services.CreateKafkaBus();
        Task task = kafkaBus.StartAsync();
        task.Wait();

        return app;
    }
}