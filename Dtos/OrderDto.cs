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
