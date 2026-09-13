namespace SalesDB.Dtos;


public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Password { get; set; }
}
public class RegisterEmployee
{
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Password { get; set; }
}
public class LoginRequest
{
    public string? Phone { get; set; }
    public string? Password { get; set; }
}