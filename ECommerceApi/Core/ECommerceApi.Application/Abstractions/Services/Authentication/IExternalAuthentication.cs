namespace ECommerceApi.Application.Abstractions.Services.Authentication;

public interface IExternalAuthentication
{
    Task<DTOs.Token> GoogleLoginAsync(string idToken, string provider, int accessTokenLifeTime);
    Task<DTOs.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
}