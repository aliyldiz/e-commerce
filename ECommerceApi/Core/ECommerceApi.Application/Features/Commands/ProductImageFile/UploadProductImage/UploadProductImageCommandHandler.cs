using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.ProductImageFile.UploadProductImage;

public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
{
    private readonly IProductImageFileRepository _productImageFileRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStorageService _storageService;
    
    public UploadProductImageCommandHandler(IProductRepository productRepository, IStorageService storageService, IProductImageFileRepository productImageFileRepository)
    {
        _productRepository = productRepository;
        _storageService = storageService;
        _productImageFileRepository = productImageFileRepository;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.Files!);

        Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id, false);
        
        await _productImageFileRepository.BulkAdd(result.Select(r => new Domain.Entities.ProductImageFile
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() { product }
        }).ToList());
        
        await _productRepository.SaveChangesAsync();

        return new();
    }
}