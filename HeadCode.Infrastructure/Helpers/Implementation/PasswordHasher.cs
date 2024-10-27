namespace HeadCode.Infrastructure.Helpers.Implementation;

using Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Models;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHashOptions _options;

    public PasswordHasher(IOptions<PasswordHashOptions> options)
    {
        _options = options.Value;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, _options.WorkFactor);
    }


    public PasswordVerificationResult VerifyHashedPassword(string providedPassword, string hashedPassword)
    {
        bool result = BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);

        return result ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}