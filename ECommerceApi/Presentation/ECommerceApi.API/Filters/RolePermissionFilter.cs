using System.Reflection;
using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ECommerceApi.API.Filters;

public class RolePermissionFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;

    public RolePermissionFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var name = context.HttpContext.User.Identity?.Name;
        if (!string.IsNullOrEmpty(name) && name != "admin")
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attributes = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
            
            var httpMethodAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

            var code =
                $"{(httpMethodAttribute != null ? httpMethodAttribute.HttpMethods.First() : HttpMethods.Get)}.{attributes.ActionType}.{attributes.Definition.Replace(" ", "")}";

            var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

            if (!hasRole)
                context.Result = new UnauthorizedResult();
            else
                await next();
        }
        else
            await next();
    }
}