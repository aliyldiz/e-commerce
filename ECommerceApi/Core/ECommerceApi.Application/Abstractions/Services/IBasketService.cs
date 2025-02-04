using ECommerceApi.Application.ViewModels.Baskets;
using ECommerceApi.Domain.Entities;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IBasketService
{
    public Task<List<BasketItem>> GetBasketItemsAsync();
    public Task AddItemToBasketAsync(CreateBasketItemViewModel basketItem);
    public Task UpdateBasketQuantityAsync(UpdateBasketItemViewModel basketItem);
    public Task RemoveBasketItemAsync(string id);
    public Basket? GetUserActiveBasket { get; }
}