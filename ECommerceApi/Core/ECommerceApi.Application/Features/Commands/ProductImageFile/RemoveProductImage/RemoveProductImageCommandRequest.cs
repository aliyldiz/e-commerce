using MediatR;

namespace ECommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandRequest : IRequest<RemoveProductImageCommandResponse>
{
    public required string Id { get; set; }
    public string? ImageId { get; set; }
}