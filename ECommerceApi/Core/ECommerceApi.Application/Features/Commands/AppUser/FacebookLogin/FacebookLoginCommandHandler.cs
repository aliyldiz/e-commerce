using ECommerceApi.Application.Abstractions.Services.Authentication;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    private readonly IExternalAuthentication _externalAuthentication;

    public FacebookLoginCommandHandler(IExternalAuthentication externalAuthentication)
    {
        _externalAuthentication = externalAuthentication;
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _externalAuthentication.FacebookLoginAsync(request.AuthToken, 900);

        return new()
        {
            Token = token
        };
    }
}