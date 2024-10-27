namespace HeadCode.Infrastructure.Helpers.Abstract;

using Microsoft.AspNetCore.Identity;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public PasswordVerificationResult VerifyHashedPassword(string providedPassword, string hashedPassword);
}