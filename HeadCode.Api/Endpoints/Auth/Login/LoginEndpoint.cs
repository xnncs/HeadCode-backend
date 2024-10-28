namespace HeadCode.Api.Endpoints.Auth.Login;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Infrastructure.Helpers.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class LoginEndpoint : Endpoint<LoginRequest, Results<Ok, BadRequest<string>>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<LoginEndpoint> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public LoginEndpoint(ApplicationDbContext dbContext, ILogger<LoginEndpoint> logger, IPasswordHasher passwordHasher,
                         IJwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }


    public override void Configure()
    {
        AllowAnonymous();
        Post("api/auth/login");
    }

    public override async Task<Results<Ok, BadRequest<string>>> ExecuteAsync(
        LoginRequest request, CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("User {Login} not found", request.Login);
            return TypedResults.BadRequest("Invalid login");
        }

        PasswordVerificationResult state = _passwordHasher.VerifyHashedPassword(
            request.Password,
            user.PasswordHash);

        if (state == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("User {Login} verification failed (bad password), password was {Password}",
                request.Login, request.Password);
            return TypedResults.BadRequest("Invalid password");
        }

        string token = _jwtProvider.GenerateToken(user);
        HttpContext.Response.Cookies.Append("tasty-cookies", token);

        _logger.LogInformation("User {Login} logged in", request.Login);

        return TypedResults.Ok();
    }
}