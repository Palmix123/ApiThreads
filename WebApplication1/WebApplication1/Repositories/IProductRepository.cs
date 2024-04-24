namespace WebApplication1.Repositories;

public interface IProductRepository
{
    Task<bool> DoesProductExist(int id);
}