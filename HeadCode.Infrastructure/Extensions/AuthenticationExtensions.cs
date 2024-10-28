namespace HeadCode.Infrastructure.Extensions;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services,
                                                          IConfiguration configuration)
    {
        JwtOptions jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>() ??
                                throw new Exception("Wrong jwtOptions format");
        string secretKey = jwtOptions.SecretKey;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(secretKey))
                     };

                     options.Events = new JwtBearerEvents
                     {
                         OnMessageReceived = context =>
                         {
                             context.Token = context.Request.Cookies["tasty-cookies"];

                             return Task.CompletedTask;
                         }
                     };
                 });

        services.AddAuthorization();

        return services;
    }
}