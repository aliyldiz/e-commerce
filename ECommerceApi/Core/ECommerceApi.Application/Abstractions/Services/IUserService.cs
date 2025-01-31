using ECommerceApi.Application.DTOs.User;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
}