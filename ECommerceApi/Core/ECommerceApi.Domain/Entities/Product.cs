using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<ProductImageFile>? ProductImageFiles { get; set; }
}