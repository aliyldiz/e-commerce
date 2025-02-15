using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Domain.Entities.Identity;

public class AppRole : IdentityRole<string>
{
    public ICollection<Endpoint> Endpoints { get; set; }
}