using MediatR;

namespace ECommerceApi.Application.Features.Commands.AppUser.PasswordReset;

public class PasswordResetCommandRequest : IRequest<PasswordResetCommandResponse>
{
    public string Email { get; set; }
}