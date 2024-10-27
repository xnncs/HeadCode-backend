namespace HeadCode.Infrastructure.Helpers.Absract;

using Core.Models;

public interface IJwtProvider
{
    string GenerateToken(User user);
    string GetIdFromClaims(string jwtToken);
}