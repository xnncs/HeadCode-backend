namespace HeadCode.Infrastructure.Helpers.Abstract;

using HeadCode.Core.Models;

public interface IJwtProvider
{
    string GenerateToken(User user);
    string GetIdFromClaims(string jwtToken);
}