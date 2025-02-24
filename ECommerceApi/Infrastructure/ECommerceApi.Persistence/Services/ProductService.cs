using System.Text.Json;
using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;

namespace ECommerceApi.Persistence.Services;

public class ProductService : IProductService
{
    readonly IProductRepository _productReadRepository;
    readonly IQRCodeService _qrCodeService;
    
    public ProductService(IProductRepository productReadRepository, IQRCodeService qrCodeService)
    {
        _productReadRepository = productReadRepository;
        _qrCodeService = qrCodeService;
    }

    public async Task<byte[]> QrCodeToProductAsync(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found");

        var plainObject = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        };
        string plainText = JsonSerializer.Serialize(plainObject);

        return _qrCodeService.GenerateQRCode(plainText);
    }
}