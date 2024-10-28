namespace HeadCode.Infrastructure.Extensions;

using Helpers.Abstract;
using Helpers.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        // adding helpers 
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        // configuring options 
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<PasswordHashOptions>(configuration.GetSection(nameof(PasswordHashOptions)));

        services.AddApiAuthentication(configuration);

        return services;
    }
}