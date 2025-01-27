using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Application.Features.Commands.AppUser.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

    public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        IdentityResult result = await _userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName
        }, request.Password);

        CreateUserCommandResponse response = new() { Succeded = result.Succeeded };

        if (result.Succeeded)
            response.Message = "User created successfully";
        else
            foreach (IdentityError error in result.Errors)
                response.Message += $"{error.Code} - {error.Description}\n";
        
        return response;
    }
}