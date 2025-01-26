namespace ECommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages;

public class GetProductImagesQueryResponse
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public required string Path { get; set; }
}