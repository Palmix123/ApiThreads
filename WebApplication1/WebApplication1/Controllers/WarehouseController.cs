using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    public WarehouseController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
}