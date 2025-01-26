namespace ECommerceApi.Domain.Entities;

public class ProductImageFile : File
{
    public required ICollection<Product> Products { get; set; }
}