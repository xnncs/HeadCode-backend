namespace HeadCode.Api.Kafka.Configuration;

using KafkaFlow;
using KafkaFlow.Serializer;

public static class KafkaConfigurationExtensions
{
    public static IServiceCollection AddConfiguredKafka(this IServiceCollection services)
    {
        const string topicName = "tests-solving-topic";
        const string producerName = "headcode-api-tests-solving-topic";
        const string consumerServer = "kafka";
        const string consumerPort = "9092";

        services.AddKafka(
            kafka => kafka
               .AddCluster(
                    cluster => cluster
                              .WithBrokers(new[] { $"{consumerServer}:{consumerPort}" })
                              .CreateTopicIfNotExists(topicName, 1, 1)
                              .AddProducer(
                                   producerName,
                                   producer =>
                                       producer.DefaultTopic(topicName)
                                               .AddMiddlewares(middlewares => 
                                                    middlewares.AddSerializer<JsonCoreSerializer>())
                               )
                )
        );
        
        return services;
    }
}