using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandRequest : IRequest<FacebookLoginCommandResponse>
{
    public string AuthToken { get; set; }
}