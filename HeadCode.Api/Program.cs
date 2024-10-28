using HeadCode.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

services.ConfigureDependencyInjection(configuration);

WebApplication app = builder.Build();

app.ConfigureHttpPipeline();

app.Run();