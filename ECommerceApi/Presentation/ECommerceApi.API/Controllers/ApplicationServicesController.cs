using ECommerceApi.Application.Abstractions.Services.Configuration;
using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
public class ApplicationServicesController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationServicesController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet]
    [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Authorize Definition Endpoints", Menu = "Application Services")]
    public IActionResult GetAuthorizeDefinitionEndpoints()
    {
        var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
        return Ok(datas);
    }
}