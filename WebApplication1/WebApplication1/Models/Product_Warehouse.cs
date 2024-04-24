namespace WebApplication1.Models;

public class Product_Warehouse
{
    private int IdProductWarehouse { get; set; }
    private int IdWarehouse { get; set; }
    private int IdProduct { get; set; }
    private int Amount { get; set; }
    private double Price { get; set; }
    private DateTime CreatedAt { get; set; }
}