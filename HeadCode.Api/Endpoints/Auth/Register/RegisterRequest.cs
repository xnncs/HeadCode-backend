namespace HeadCode.Api.Endpoints.Auth.Register;

public class RegisterRequest
{
    public string Login { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
    
}