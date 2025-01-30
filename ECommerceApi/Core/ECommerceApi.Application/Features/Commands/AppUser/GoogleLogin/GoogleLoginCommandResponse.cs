using ECommerceApi.Application.DTOs;

namespace ECommerceApi.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandResponse
{
    public Token Token { get; set; }
}