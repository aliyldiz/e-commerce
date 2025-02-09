using ECommerceApi.Application.Enums;

namespace ECommerceApi.Application.CustomAttributes;

public class AuthorizeDefinitionAttribute : Attribute
{
    public string Menu { get; set; }
    public string Definition { get; set; }
    public ActionType ActionType { get; set; }
}