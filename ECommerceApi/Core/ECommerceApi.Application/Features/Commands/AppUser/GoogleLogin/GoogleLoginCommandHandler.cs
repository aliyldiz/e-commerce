using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Abstractions.Services.Authentication;
using ECommerceApi.Application.DTOs;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    private readonly IExternalAuthentication _externalAuthentication;

    public GoogleLoginCommandHandler(IExternalAuthentication externalAuthentication)
    {
        _externalAuthentication = externalAuthentication;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        Token token = await _externalAuthentication.GoogleLoginAsync(request.IdToken, request.Provider, 15);
        return new()
        {
            Token = token
        };
    }
}