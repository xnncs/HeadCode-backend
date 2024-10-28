namespace HeadCode.DataAccess.Extensions;

using DatabaseContexts;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}