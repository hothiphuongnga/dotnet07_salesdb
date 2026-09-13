using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
}

public class OrderRepository(SalesDbContext _db) : IOrderRepository
{
    public async Task<List<Order>> GetAllAsync()
    {
        var orders = await _db.Orders.ToListAsync();
        return orders;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(p => p.Id == id);
        return order;
    }
}