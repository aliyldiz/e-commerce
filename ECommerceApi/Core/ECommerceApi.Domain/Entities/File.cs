using System.ComponentModel.DataAnnotations.Schema;
using ECommerceApi.Domain.Entities.Common;

namespace ECommerceApi.Domain.Entities;

public class File : BaseEntity
{
    public required string FileName { get; set; }
    public required string Path { get; set; }
    public required string Storage { get; set; }
    
    [NotMapped] // File tablosunda ModifiedeDate'i kullanılmasını istemiyoruz. Ayrıca Base Class'ta ki property'nin her Child Class'ta kullanılmasını opsiyonlamak için virtual ile işaretlenmelidir.
    public override DateTime ModifiedDate { get; set; }
}