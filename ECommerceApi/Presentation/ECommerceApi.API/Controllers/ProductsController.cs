using Bogus;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public ProductsController(IProductRepository productRepository, IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    [HttpGet]
    [Route("get")]
    public async Task Get()
    {
        var spec = "b360cb2d-3acd-4f73-8d4f-9883abe61eee";
        var specid = Guid.Parse(spec);
        Order a = await _orderRepository.GetByIdAsync(specid,noTracking:false);
        Console.WriteLine(a.Address);
        a.Address = "kurucesme";
        await _orderRepository.SaveChangesAsync(); 
        Console.WriteLine(a.Address);
    }
    
    [HttpGet]
    [Route("getsp")]
    public async Task<IActionResult> Getsp()
    {
        var spec = "b360cb2d-3acd-4f73-8d4f-9883abe61eee";
        var specid = Guid.Parse(spec);
        Order a = await _orderRepository.GetByIdAsync(specid,noTracking:false);
        return Ok(a.Address);
    }
}