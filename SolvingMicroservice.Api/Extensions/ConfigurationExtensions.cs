namespace SolvingMicroservice.Api.Extensions;

using Kafka.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services,
                                                                  IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddConfiguredKafka();
        
        return services;
    }

    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseKafka();
        
        app.MapGet("/", () => "Working...");
        
        return app;
    }
}