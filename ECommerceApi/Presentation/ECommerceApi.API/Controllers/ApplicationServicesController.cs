using ECommerceApi.Application.Abstractions.Services.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationServicesController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationServicesController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet]
    public IActionResult GetAuthorizeDefinitionEndpoints()
    {
        var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
        return Ok(datas);
    }
}