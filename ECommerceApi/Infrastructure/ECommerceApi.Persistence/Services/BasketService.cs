using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Application.ViewModels.Baskets;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Services;

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderRepository _orderRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;

    public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderRepository orderRepository, IBasketRepository basketRepository, IBasketItemRepository basketItemRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
    }

    private async Task<Basket?> ContextUser()
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userName))
        {
            AppUser? user = await _userManager.Users
                .Include(u => u.Baskets)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            var _basket = from basket in user.Baskets
                join order in _orderRepository.dbSet on basket.Id equals order.Id into BasketOrders
                from order in BasketOrders.DefaultIfEmpty()
                select new
                {
                    Basket = basket,
                    Order = order
                };

            Basket? targetBasket = null;
            if (_basket.Any(b => b.Order is null))
                targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
            else
            {
                targetBasket = new();
                user.Baskets.Add(targetBasket);
            }
            
            await _basketRepository.SaveChangesAsync();
            return targetBasket;
        }

        throw new Exception("Something went wrong");
    }

    public async Task<List<BasketItem>> GetBasketItemsAsync()
    {
        Basket? basket = await ContextUser();
        Basket? result = await _basketRepository.dbSet.Include(b => b.BasketItems)
            .ThenInclude(bi => bi.Product)
            .FirstOrDefaultAsync(b => b.Id == basket.Id); 
        
        return result.BasketItems.ToList();
    }

    public async Task AddItemToBasketAsync(CreateBasketItemViewModel basketItem)
    {
        Basket? basket = await ContextUser();
        if (basket != null)
        {
            BasketItem _basketItem = await _basketItemRepository.GetSingleAsync(bi =>
                bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
            
            if(_basketItem != null)
                _basketItem.Quantity++;
            else
                await _basketItemRepository.AddAsync(new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = Guid.Parse(basketItem.ProductId),
                    Quantity = basketItem.Quantity
                });
            
            await _basketItemRepository.SaveChangesAsync();
        }
    }

    public async Task UpdateBasketQuantityAsync(UpdateBasketItemViewModel basketItem)
    {
        BasketItem? _basketItem = await _basketItemRepository.GetByIdAsync(basketItem.BasketItemId, false);
        if (_basketItem != null)
        {
            _basketItem.Quantity = basketItem.Quantity;
            await _basketItemRepository.SaveChangesAsync();
        }
    }

    public async Task RemoveBasketItemAsync(string id)
    {
        BasketItem? basketItem = await _basketItemRepository.GetByIdAsync(id);
        if (basketItem != null)
        {
            _basketItemRepository.Delete(basketItem);
            await _basketItemRepository.SaveChangesAsync();
        }
    }
}