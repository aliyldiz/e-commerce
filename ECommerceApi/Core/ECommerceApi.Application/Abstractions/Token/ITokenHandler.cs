using ECommerceApi.Domain.Entities.Identity;

namespace ECommerceApi.Application.Abstractions.Token;

public interface ITokenHandler
{
    DTOs.Token CreateAccessToken(int second, AppUser user);
    string CreateRefreshToken();
}