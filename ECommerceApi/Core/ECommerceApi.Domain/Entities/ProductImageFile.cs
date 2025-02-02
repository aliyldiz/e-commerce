namespace ECommerceApi.Domain.Entities;

public class ProductImageFile : File
{
    public bool Showcase { get; set; }
    public required ICollection<Product> Products { get; set; }
}