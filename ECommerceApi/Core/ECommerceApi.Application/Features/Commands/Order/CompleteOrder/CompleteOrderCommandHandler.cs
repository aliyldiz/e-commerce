using ECommerceApi.Application.Abstractions.Services;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Order.CompleteOrder;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
{
    private readonly IOrderService _service;

    public CompleteOrderCommandHandler(IOrderService service)
    {
        _service = service;
    }

    public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _service.CompleteOrderAsync(request.Id);
        return new();
    }
}