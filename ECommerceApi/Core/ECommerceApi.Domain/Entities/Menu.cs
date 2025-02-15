using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class Menu : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Endpoint> Endpoints { get; set; }
}