using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 FROM Product WHERE ID = @ID";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }
}