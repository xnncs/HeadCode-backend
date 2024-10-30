namespace HeadCode.Api.Extensions;

using DataAccess.Extensions;
using Endpoints.Problems.Add;
using FastEndpoints;
using FluentValidation;
using Helpers.Abstract;
using Helpers.Implementation;
using Infrastructure.Extensions;
using Kafka.Configuration;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.HttpResults;
using Validation;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services,
                                                                  IConfiguration configuration)
    {
        services.AddDataAccessServices();
        services.AddInfrastructureServices(configuration);
        
        services.AddScoped<IAuthHelper, AuthHelper>();

        services.AddMapster();

        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddFastEndpoints();

        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        
        services.AddMediatR(x =>
                x.RegisterServicesFromAssemblyContaining<Program>()
                 .AddBehavior<IPipelineBehavior<AddProblemRequest, Results<Created, BadRequest<string>>>,
            ValidationBehaviour<AddProblemRequest, Created>>()
            );

        services.AddConfiguredKafka();
        
        return services;
    }

    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
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