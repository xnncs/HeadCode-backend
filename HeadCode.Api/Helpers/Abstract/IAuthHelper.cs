namespace HeadCode.Api.Helpers.Abstract;

using Core.Models;

public interface IAuthHelper
{
     Task<User?> GetUserFromCookiesAsync(HttpContext httpContext);
}