using ECommerceApi.Application.DTOs.Configuration;

namespace ECommerceApi.Application.Abstractions.Services.Configuration;

public interface IApplicationService
{
    List<Menu> GetAuthorizeDefinitionEndpoints(Type assemblyType);
}