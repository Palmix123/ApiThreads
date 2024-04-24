namespace WebApplication1.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> DoesWarehouseExist(int id);
}