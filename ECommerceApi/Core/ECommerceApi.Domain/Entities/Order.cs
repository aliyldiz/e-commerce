using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class Order : BaseEntity
{
    // public Guid CustomerId { get; set; }
    public required string Description { get; set; }
    public required string Address { get; set; }
    public Basket Basket { get; set; }
    // public required ICollection<Product> Products { get; set; }
    // public required Customer Customer { get; set; }
}