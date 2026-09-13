namespace SalesDB.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}

public class AddProductRequest 
{
    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }
}