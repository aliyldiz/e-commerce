using System.Net;
using ECommerceApi.Application.Features.Commands.Product.CreateProduct;
using ECommerceApi.Application.Features.Commands.Product.RemoveProduct;
using ECommerceApi.Application.Features.Commands.Product.UpdateProduct;
using ECommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerceApi.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceApi.Application.Features.Queries.Product.GetAllProduct;
using ECommerceApi.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")] // default olarak tanımlama yapsaydık bildirmemize gerek kalmayacaktı
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest requestModel)
    {
        GetAllProductQueryResponse allProducts = await _mediator.Send(requestModel);
        return Ok(allProducts);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest requestModel)
    {
        var product = await _mediator.Send(requestModel);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateProductCommandRequest requestModel)
    {
        await _mediator.Send(requestModel);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest requestModel)
    {
        await _mediator.Send(requestModel);
        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest requestModel)
    {
        var result = await _mediator.Send(requestModel);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest requestModel)
    {
        requestModel.Files = Request.Form.Files;
        await _mediator.Send(requestModel);
        return Ok();
    }

    [HttpGet("[action]/{Id}")]
    public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest requestModel)
    {
        List<GetProductImagesQueryResponse> images = await _mediator.Send(requestModel);
        return Ok(images);
    }

    [HttpDelete("[action]/{Id}")]
    public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest requestModel, [FromQuery] string imageId)
    {
        requestModel.ImageId = imageId;
        await _mediator.Send(requestModel);
        return Ok();
    }
}