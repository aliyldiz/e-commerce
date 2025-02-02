using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;

public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
{
    private readonly IProductImageFileRepository _productImageFileRepository;

    public ChangeShowcaseImageCommandHandler(IProductImageFileRepository productImageFileRepository)
    {
        _productImageFileRepository = productImageFileRepository;
    }

    public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
    {
        var query = _productImageFileRepository.dbSet.Include(p => p.Products).SelectMany(p => p.Products,
            (productImageFile, productId) => new
            {
                productImageFile, productId
            });
        var data = await query.FirstOrDefaultAsync(p => p.productId.Id == Guid.Parse(request.ProductId) && p.productImageFile.Showcase);
        
        if (data != null)
            data.productImageFile.Showcase = false;
        
        var image = await query.FirstOrDefaultAsync(p => p.productImageFile.Id == Guid.Parse(request.ImageId));
        
        if (image != null)
            image.productImageFile.Showcase = true;

        await _productImageFileRepository.SaveChangesAsync();

        return new();
    }
}