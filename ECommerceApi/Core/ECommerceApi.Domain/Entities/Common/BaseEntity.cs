namespace ECommerceApi.Domain.Entities.Common;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
}