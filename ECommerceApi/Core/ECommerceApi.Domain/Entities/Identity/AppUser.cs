using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Domain.Entities.Identity;

public class AppUser : IdentityUser<string>
{
    public string FullName { get; set; }
}