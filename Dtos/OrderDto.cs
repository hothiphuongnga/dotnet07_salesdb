namespace SalesDB.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? Date { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? EmployeeId { get; set; }

    public string Status { get; set; } = null!;
}
public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}