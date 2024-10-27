namespace HeadCode.Api.Helpers.Implementation;

using Abstract;
using Core.Models;
using DataAccess.DatabaseContexts;
using Infrastructure.Helpers.Absract;
using Microsoft.EntityFrameworkCore;

public class AuthHelper : IAuthHelper
{
    public AuthHelper(IJwtProvider jwtProvider, ApplicationDbContext dbContext)
    {
        _jwtProvider = jwtProvider;
        _dbContext = dbContext;
    }
    
    private readonly IJwtProvider _jwtProvider;
    private readonly ApplicationDbContext _dbContext;

    public async Task<User?> GetUserFromCookiesAsync(HttpContext httpContext)
    {
        string? jwtTokenFromCookies = httpContext.Request.Cookies["tasty-cookies"];
        if (string.IsNullOrEmpty(jwtTokenFromCookies))
        {
            return null;
        }
        
        string stringResult = _jwtProvider.GetIdFromClaims(jwtTokenFromCookies);
        bool isConverted = Guid.TryParse(stringResult, out Guid id);
        if (!isConverted)
        {
            throw new Exception($"Unable to convert {jwtTokenFromCookies} to int");
        }

        return await _dbContext.Users.AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == id);
    }
}