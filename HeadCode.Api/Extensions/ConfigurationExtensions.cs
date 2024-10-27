namespace HeadCode.Api.Extensions;

using DataAccess.Extensions;
using FastEndpoints;
using Helpers.Abstract;
using Helpers.Implementation;
using Infrastructure.Extensions;
using Mapster;
using Microsoft.AspNetCore.CookiePolicy;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services,
                                                                  IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddDataAccessServices();
        
        services.AddInfrastructureServices(configuration);

        services.AddScoped<IAuthHelper, AuthHelper>();
        
        services.AddMapster();
        
        return services;
    }

    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseRouting();
        app.UseHttpsRedirection();
        
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapFastEndpoints();

        return app;
    }
}