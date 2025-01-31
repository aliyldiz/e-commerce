using ECommerceApi.Application.Abstractions.Services.Authentication;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IAuthService : IInternalAuthentication, IExternalAuthentication
{
    
}