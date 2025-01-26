using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
{
    private readonly IProductRepository _productRepository;

    public RemoveProductImageCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _productRepository.dbSet.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
        
        Domain.Entities.ProductImageFile? productImageFile = (product?.ProductImageFiles!).FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId!));
        
        if (productImageFile != null)
            (product?.ProductImageFiles!).Remove(productImageFile);
        
        await _productRepository.SaveChangesAsync();
        return new();
    }
}