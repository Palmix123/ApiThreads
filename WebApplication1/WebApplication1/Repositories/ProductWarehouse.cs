using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class ProductWarehouse : IProductWarehouse
{
    private readonly IConfiguration _configuration;
    public ProductWarehouse(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesOrderExistInProduct_Warehouse(int idOrder)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdOrder", idOrder);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }

    public async Task AddRecordToProduct_Warehouse(int idProduct, int idWarehouse, int amount, DateTime createdAt, int idOrder)
    {
        var query = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idProduct);

        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();
        decimal price = 0;
        if (await reader.ReadAsync())
        {
            var priceOrdinal = reader.GetOrdinal("Price");
            price = reader.GetDecimal(priceOrdinal);
        }
        
        await connection.CloseAsync();
        price *= amount;
        
        var query2 = "INSERT INTO Product_Warehouse VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";

        await using SqlConnection connection2 = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command2 = new SqlCommand();
        // int idProduct, int idWarehouse, int amount, DateTime createdAt, int idOrder
        command2.Connection = connection2;
        command2.CommandText = query2;
        command2.Parameters.AddWithValue("@Price", price);
        command2.Parameters.AddWithValue("@IdProduct", idProduct);
        command2.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        command2.Parameters.AddWithValue("@Amount", amount);
        command2.Parameters.AddWithValue("@CreatedAt", createdAt);
        command2.Parameters.AddWithValue("@IdOrder", idOrder);

        await connection2.OpenAsync();
        await command2.ExecuteNonQueryAsync();
        await connection2.CloseAsync();
    }
}