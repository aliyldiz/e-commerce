using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Abstractions.Services.Configuration;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Services;

public class AuthorizationEndpointService : IAuthorizationEndpointService
{
    private readonly IApplicationService _applicationService;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly RoleManager<AppRole> _roleManager;

    public AuthorizationEndpointService(IApplicationService applicationService, IEndpointRepository endpointRepository, IMenuRepository menuRepository, RoleManager<AppRole> roleManager)
    {
        _applicationService = applicationService;
        _endpointRepository = endpointRepository;
        _menuRepository = menuRepository;
        _roleManager = roleManager;
    }

    public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
    {
        var _menu = await _menuRepository.dbSet.FirstOrDefaultAsync(m => m.Name == menu) 
                    ?? new Menu { Id = Guid.NewGuid(), Name = menu };

        if (_menu.Id == Guid.Empty)
        {
            await _menuRepository.AddAsync(_menu);
        }

        var endpoint = await _endpointRepository.dbSet
            .Include(e => e.Menu)
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

        if (endpoint == null)
        {
            var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
                .FirstOrDefault(m => m.Name == menu)?
                .Actions.FirstOrDefault(e => e.Code == code);

            if (action == null)
            {
                throw new InvalidOperationException($"Action with code {code} not found in menu {menu}.");
            }

            endpoint = new Endpoint
            {
                Code = action.Code,
                ActionType = action.ActionType,
                HttpType = action.HttpType,
                Definition = action.Definition,
                Id = Guid.NewGuid(),
                Menu = _menu
            };

            await _endpointRepository.AddAsync(endpoint);
        }

        var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();
        endpoint.Roles.Clear();
        foreach (var role in appRoles)
        {
            endpoint.Roles.Add(role);
        }

        await _endpointRepository.SaveChangesAsync();
    }

    public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
    {
        Endpoint? endpoint = await _endpointRepository.dbSet
            .Include(e => e.Roles)
            .Include(e => e.Menu)
            .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
        if (endpoint != null)
            return endpoint.Roles.Select(r => r.Name).ToList();
        return null;
    }
}