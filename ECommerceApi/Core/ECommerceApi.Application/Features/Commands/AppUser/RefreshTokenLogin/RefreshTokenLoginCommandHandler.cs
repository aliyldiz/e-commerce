using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
    {
        Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
        return new()
        {
            Token = token,
        };
    }
}