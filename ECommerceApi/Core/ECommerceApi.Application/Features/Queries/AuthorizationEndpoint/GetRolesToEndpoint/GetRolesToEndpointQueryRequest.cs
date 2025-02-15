using MediatR;

namespace ECommerceApi.Application.Features.Queries.AuthorizationEndpoint;

public class GetRolesToEndpointQueryRequest : IRequest<GetRolesToEndpointQueryResponse>
{
    public string Code { get; set; }
    public string Menu { get; set; }
}