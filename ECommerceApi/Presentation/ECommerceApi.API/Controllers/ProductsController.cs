using System.Net;
using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Application.RequestParameters;
using ECommerceApi.Application.ViewModels.Products;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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
    
    public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, IProductImageFileRepository productImageFileRepository, IFileRepository fileRepository, IInvoiceFileRepository invoiceFileRepository, IStorageService storageService)
    {
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
        _productImageFileRepository = productImageFileRepository;
        _fileRepository = fileRepository;
        _invoiceFileRepository = invoiceFileRepository;
        _storageService = storageService;
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
        // var datas = await _fileService.UploadFileAsync("resource/product-images", Request.Form.Files);
        // await _productImageFileRepository.BulkAdd(datas.Select(d => new ProductImageFile()
        // {
        //     FileName = d.fileName,
        //     Path = d.path
        // }).ToList());
        // await _productImageFileRepository.SaveChangesAsync();
        
        // var datas = await _fileService.UploadFileAsync("resource/invoices", Request.Form.Files);
        // await _invoiceFileRepository.BulkAdd(datas.Select(i => new InvoiceFile()
        // {
        //     FileName = i.fileName,
        //     Path = i.path,
        //     Price = new Random().Next(1000, 10000)
        // }).ToList());
        // await _invoiceFileRepository.SaveChangesAsync();
        
        // var datas = await _fileService.UploadFileAsync("resource/files", Request.Form.Files);
        // await _fileRepository.BulkAdd(datas.Select(f => new File()
        // {
        //     FileName = f.fileName,
        //     Path = f.path,
        // }).ToList());
        // await _fileRepository.SaveChangesAsync();

        var files = await _fileRepository.GetAllAsync(); // file, productImage, invoice olmak üzere hepsini getirir. capacity: 32
        var productImages = await _productImageFileRepository.GetAllAsync(); // sadece productImage'i getirir. capacity: 4
        var invoices = await _invoiceFileRepository.GetAllAsync(); // sadece invoices'i getirir. capacity: 16
        
        var datas = await _storageService.UploadAsync("resource/files", Request.Form.Files);
        await _productImageFileRepository.BulkAdd(datas.Select(d => new ProductImageFile()
        {
            FileName = d.fileName,
            Path = d.pathOrContainerName,
            Storage = _storageService.StorageName
        }).ToList());
        await _productImageFileRepository.SaveChangesAsync();
        
        return Ok();
    }
}