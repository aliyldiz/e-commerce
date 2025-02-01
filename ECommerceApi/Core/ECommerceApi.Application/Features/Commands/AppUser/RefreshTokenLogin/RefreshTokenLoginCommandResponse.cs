using ECommerceApi.Application.DTOs;

namespace ECommerceApi.Application.Features.Commands.AppUser.RefreshTokenLogin;

public class RefreshTokenLoginCommandResponse
{
    public Token Token { get; set; }
}