using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class CompletedOrder : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}