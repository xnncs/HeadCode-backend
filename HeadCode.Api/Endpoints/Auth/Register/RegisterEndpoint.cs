namespace HeadCode.Api.Endpoints.Auth.Register;

using Core.Models;
using DataAccess.DatabaseContexts;
using FastEndpoints;
using Infrastructure.Helpers.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class RegisterEndpoint : Endpoint<RegisterRequest, Results<Ok, BadRequest<string>, InternalServerError>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RegisterEndpoint> _logger;

    private readonly IPasswordHasher _passwordHasher;

    public RegisterEndpoint(ApplicationDbContext dbContext, ILogger<RegisterEndpoint> logger,
                            IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }


    public override void Configure()
    {
        AllowAnonymous();
        Post("api/auth/register");
    }

    public override async Task<Results<Ok, BadRequest<string>, InternalServerError>> ExecuteAsync(
        RegisterRequest request, CancellationToken cancellationToken)
    {
        bool emailAlreadyUsed = await _dbContext.Users
                                                .AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailAlreadyUsed) return TypedResults.BadRequest<string>("Email already used");

        bool loginAlreadyUsed = await _dbContext.Users
                                                .AnyAsync(x => x.Login == request.Login, cancellationToken);
        if (loginAlreadyUsed) return TypedResults.BadRequest<string>("Login already exists");

        string passwordHash = _passwordHasher.HashPassword(request.Password);
        User created = Core.Models.User.Create(request.Username, request.Login, request.Email, passwordHash);


        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _dbContext.Users.Add(created);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok();
    }
}