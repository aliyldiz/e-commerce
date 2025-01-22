using System.Net;
using Bogus;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Application.RequestParameters;
using ECommerceApi.Application.ViewModels.Products;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
    {
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]Pagination pagination)
    {
        var totalCount = _productRepository.Get(null, true, null).Count();
        var products = _productRepository.Get(null, true, null).Skip((pagination.Page) * pagination.Size).Take(pagination.Size).ToList();
        
        return Ok(new
        {
            totalCount,
            products
        });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _productRepository.GetByIdAsync(Guid.Parse(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProductCreateVM model)
    {
        await _productRepository.AddAsync(new Product()
        {
            Name = model.Name,
            Price = model.Price,
            Stock = model.Stock
        });
        await _productRepository.SaveChangesAsync();
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> Put(ProductUpdateVM model)
    {
        var product = await _productRepository.GetByIdAsync(model.Id, false);
        product.Name = model.Name;
        product.Price = model.Price;
        product.Stock = model.Stock;
        await _productRepository.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productRepository.DeleteAsync(Guid.Parse(id));
        await _productRepository.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Upload()
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        
        Random random = new();

        foreach (IFormFile file in Request.Form.Files)
        {
            string fullPath = Path.Combine(uploadPath, $"{random.Next()}{Path.GetExtension(file.FileName)}");

            using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 1024*1024, useAsync: false);
            
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
        }
        return Ok();
    }
}