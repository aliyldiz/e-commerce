using ECommerceApi.Application.Abstractions.Services;
using MediatR;

namespace ECommerceApi.Application.Features.Queries.Order.GetAllOrder;

public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQueryRequest, GetAllOrderQueryResponse>
{
    private readonly IOrderService _service;

    public GetAllOrderQueryHandler(IOrderService service)
    {
        _service = service;
    }

    public async Task<GetAllOrderQueryResponse> Handle(GetAllOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _service.GetAllOrderAsync(request.Page, request.Size);

        return new()
        {
            TotalOrderCount = data.TotalOrderCount,
            Orders = data.Orders
        };
    }
}