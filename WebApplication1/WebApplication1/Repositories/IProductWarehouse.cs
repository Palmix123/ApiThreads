namespace WebApplication1.Repositories;

public interface IProductWarehouse
{
    public Task<bool> DoesOrderExistInProduct_Warehouse(int idOrder);
    public Task AddRecordToProduct_Warehouse(int idProduct, int idWarehouse, int amount, DateTime createdAt, int idOrder);
}