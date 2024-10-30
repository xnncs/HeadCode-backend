namespace SolvingMicroservice.DataAccess.Extensions;

using Microsoft.Extensions.DependencyInjection;
using SolvingMicroservice.DataAccess.DatabaseContexts;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}