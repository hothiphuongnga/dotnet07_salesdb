using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Product>> GetProductsAsync(IEnumerable<int> productIds);
    Task<Order> CreateAsync(Order order);
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


    public async Task<List<Product>> GetProductsAsync(IEnumerable<int> productIds)
    {
        return await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();
            
    }

    // lafm viec voiw entity 
    // 
    public async Task<Order> CreateAsync(Order order)
    {
        // neeus co truyen orderdetail thi EF se tu dong them vao CSDL cung voi order

        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        return order;
    }
}