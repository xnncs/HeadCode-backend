namespace HeadCode.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    
    public DateTime DateCreated { get; set; }

    
    public static User Create(string username, string login, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Login = login,
            Email = email,
            PasswordHash = passwordHash,
            DateCreated = DateTime.UtcNow
        };
    }
}