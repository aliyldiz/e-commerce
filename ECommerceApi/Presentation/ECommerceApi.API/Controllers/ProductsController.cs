using System.Net;
using Bogus;
using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
using ECommerceApi.Application.Features.Commands.Product.CreateProduct;
using ECommerceApi.Application.Features.Commands.Product.RemoveProduct;
using ECommerceApi.Application.Features.Commands.Product.UpdateProduct;
using ECommerceApi.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using ECommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerceApi.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceApi.Application.Features.Queries.Product.GetAllProduct;
using ECommerceApi.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IProductService _productService;

    public ProductsController(IMediator mediator, IProductService productService)
    {
        _mediator = mediator;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest requestModel)
    {
        GetAllProductQueryResponse allProducts = await _mediator.Send(requestModel);
        return Ok(allProducts);
    }

    // [HttpGet("[action]")]
    // public async Task<IActionResult> CreateDummyProduct()
    // {
    //     var dummy = new Faker<Product>()
    //         .RuleFor(d => d.Name, f => f.Commerce.ProductName())
    //         .RuleFor(d => d.Stock, f => f.Random.Int(1, 20))
    //         .RuleFor(d => d.Price, f => f.Random.Int(5, 100))
    //         .Generate(500);
    //     
    //     await _productRepository.AddAsync(dummy);
    //     await _productRepository.SaveChangesAsync();
    //     
    //     return Ok(images);
    // }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest requestModel)
    {
        var product = await _mediator.Send(requestModel);
        return Ok(product);
    }
    
    [HttpGet("qrcode/{productId}")]
    public async Task<IActionResult> GetQrCodeToProduct([FromRoute] string productId)
    {
        var data = await _productService.QrCodeToProductAsync(productId);
        return File(data, "image/png");
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing,Definition = "Create Product")]
    public async Task<IActionResult> Post(CreateProductCommandRequest requestModel)
    {
        await _mediator.Send(requestModel);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
    public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest requestModel)
    {
        await _mediator.Send(requestModel);
        return Ok();
    }

    [HttpDelete("{Id}")]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Remove Product")]
    public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest requestModel)
    {
        var result = await _mediator.Send(requestModel);
        return Ok(result);
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]
    public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest requestModel)
    {
        requestModel.Files = Request.Form.Files;
        await _mediator.Send(requestModel);
        return Ok();
    }
    
    [HttpGet("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Products Images")]
    public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest requestModel)
    {
        List<GetProductImagesQueryResponse> images = await _mediator.Send(requestModel);
        return Ok(images);
    }

    [HttpDelete("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Remove Product Image")]
    public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest requestModel, [FromQuery] string imageId)
    {
        requestModel.ImageId = imageId;
        await _mediator.Send(requestModel);
        return Ok();
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
    public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowcaseImageCommandRequest requestModel)
    {
        ChangeShowcaseImageCommandResponse response = await _mediator.Send(requestModel);
        return Ok(response);
    }
}