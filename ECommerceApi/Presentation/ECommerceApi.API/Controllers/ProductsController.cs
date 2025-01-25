using System.Net;
using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Application.RequestParameters;
using ECommerceApi.Application.ViewModels.Products;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using File = ECommerceApi.Domain.Entities.File;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileRepository _fileRepository;
    private readonly IProductImageFileRepository _productImageFileRepository;
    private readonly IInvoiceFileRepository _invoiceFileRepository;
    private readonly IStorageService _storageService;
    private readonly IConfiguration _configuration;
    
    public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, IProductImageFileRepository productImageFileRepository, IFileRepository fileRepository, IInvoiceFileRepository invoiceFileRepository, IStorageService storageService, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
        _productImageFileRepository = productImageFileRepository;
        _fileRepository = fileRepository;
        _invoiceFileRepository = invoiceFileRepository;
        _storageService = storageService;
        _configuration = configuration;
    }

    [HttpGet]
    public Task<IActionResult> Get([FromQuery]Pagination pagination)
    {
        var totalCount = _productRepository.Get(null, true, null).Count();
        var products = _productRepository.Get(null, true, null).Skip((pagination.Page) * pagination.Size).Take(pagination.Size).ToList();
        
        return Task.FromResult<IActionResult>(Ok(new
        {
            totalCount,
            products
        }));
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
    public async Task<IActionResult> Upload(string id)
    {
        List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);

        Product product = await _productRepository.GetByIdAsync(Guid.Parse(id), false);
        
        await _productImageFileRepository.BulkAdd(result.Select(r => new ProductImageFile
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Product>() { product }
        }).ToList());
        
        await _productRepository.SaveChangesAsync();
        
        return Ok();
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetProductImages(string id)
    {
        Product? product = await _productRepository.dbSet.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
        
        return Ok(product?.ProductImageFiles.Select(p => new
        {
            Path = $"{_configuration["BaseUrlStorage"]}/{p.Path}",
            p.FileName,
            p.Id
        }));
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteProductImage(string id, string imageId)
    {
        Product? product = await _productRepository.dbSet.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
        
        ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
        
        product.ProductImageFiles.Remove(productImageFile);
        await _productRepository.SaveChangesAsync();
        return Ok();
    }
}