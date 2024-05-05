using System.Diagnostics;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductWarehouse _productWarehouse;
    public WarehouseController(IProductRepository productRepository, IWarehouseRepository warehouseRepository, IOrderRepository orderRepository, IProductWarehouse productWarehouse)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _orderRepository = orderRepository;
        _productWarehouse = productWarehouse;
    }
    
    [HttpPost]
    [Route("api/scope/{idProduct:int}/{idWarehouse:int}/{amount:int}/{createdAt:datetime}")]
    public async Task<IActionResult> AddRecordToProduct_Warehouse(int idProduct, int idWarehouse, int amount, DateTime createdAt)
    {
        Console.WriteLine("alsldlasldasda");
        if(! await _productRepository.DoesProductExist(idProduct))
            return NotFound("Product doesn't exist");

        if (!await _warehouseRepository.DoesWarehouseExist(idWarehouse))
            return NotFound("Warehouse doesn't exist");
        
        var idOrder = await _orderRepository.DoesOrderWithAmountAndThisProductExist(idProduct, amount, createdAt);
        if (idOrder == -1)
            return NotFound("Order with this amount doesn't exist or bad data");

        if (!await _orderRepository.DoesFulfilledAt(idOrder))
            return NotFound("Fulfilled at error");
        
        if (await _productWarehouse.DoesOrderExistInProduct_Warehouse(idOrder))
            return NotFound("Order exist in product_warehouse");
        
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _orderRepository.UpdateFulfilledAt(idOrder);
            await _productWarehouse.AddRecordToProduct_Warehouse(idProduct, idWarehouse, amount, createdAt, idOrder);
            
            scope.Complete();
        }
        return Ok();
    }
}