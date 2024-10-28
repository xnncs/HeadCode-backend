namespace HeadCode.Infrastructure.Helpers.Implementation;

using Abstract;
using BCrypt.Net;
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
        return BCrypt.EnhancedHashPassword(password, _options.WorkFactor);
    }


    public PasswordVerificationResult VerifyHashedPassword(string providedPassword, string hashedPassword)
    {
        bool result = BCrypt.EnhancedVerify(providedPassword, hashedPassword);

        return result ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}