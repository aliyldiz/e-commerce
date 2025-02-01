using ECommerceApi.Application.Abstractions.Services.Authentication;
using ECommerceApi.Application.DTOs;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    private readonly IInternalAuthentication _internalAuthentication;

    public LoginUserCommandHandler(IInternalAuthentication internalAuthentication)
    {
        _internalAuthentication = internalAuthentication;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        Token token =  await _internalAuthentication.LoginAsync(request.UserNameOrEmail, request.Password, 900);
        return new LoginUserSuccessCommandResponse()
        {
             Token = token
        };
    }
}