namespace WebApplication1.Repositories;

public interface IOrderRepository
{
    public Task<Int32> DoesOrderWithAmountAndThisProductExist(int idProduct, int amount, DateTime createdAt);
    public Task<bool> DoesFulfilledAt(int idOrder);
    public Task UpdateFulfilledAt(int idOrder);
}