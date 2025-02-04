using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class Customer : BaseEntity
{
    public required string Name { get; set; }
    // public required ICollection<Order> Orders { get; set; }
}