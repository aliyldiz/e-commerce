using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id, false);
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        await _productRepository.SaveChangesAsync();
        
        _logger.LogInformation("product updated...");
        return new();
    }
}