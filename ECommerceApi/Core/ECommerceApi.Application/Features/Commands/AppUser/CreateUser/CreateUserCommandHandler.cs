using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Application.Features.Commands.AppUser.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {

        CreateUserResponse response = await _userService.CreateAsync(new()
        {
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.UserName,
            Password = request.Password,
            PasswordConfirm = request.PasswordConfirm,
        });
        
        return new()
        {
            Message = response.Message,
            Succeded = response.Succeded,
        };
    }
}