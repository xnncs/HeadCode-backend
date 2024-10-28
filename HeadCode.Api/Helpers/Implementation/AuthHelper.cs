namespace HeadCode.Api.Helpers.Implementation;

using Abstract;
using Core.Models;
using DataAccess.DatabaseContexts;
using Infrastructure.Helpers.Abstract;
using Microsoft.EntityFrameworkCore;

public class AuthHelper : IAuthHelper
{
    private readonly ApplicationDbContext _dbContext;

    private readonly IJwtProvider _jwtProvider;

    public AuthHelper(IJwtProvider jwtProvider, ApplicationDbContext dbContext)
    {
        _jwtProvider = jwtProvider;
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserFromCookiesAsync(HttpContext httpContext)
    {
        string? jwtTokenFromCookies = httpContext.Request.Cookies["tasty-cookies"];
        if (string.IsNullOrEmpty(jwtTokenFromCookies)) return null;

        string stringResult = _jwtProvider.GetIdFromClaims(jwtTokenFromCookies);
        bool isConverted = Guid.TryParse(stringResult, out Guid id);
        if (!isConverted) throw new Exception($"Unable to convert {jwtTokenFromCookies} to int");

        return await _dbContext.Users.AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Id == id);
    }
}