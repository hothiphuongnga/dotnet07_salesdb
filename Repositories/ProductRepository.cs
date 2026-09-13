using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

// ket noi den DB , 
// su dung EF 
public interface IProductRepository
{
    // GetAll
    // GetById
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetById(int id);

}
public class ProductRepository(SalesDbContext _db) : IProductRepository
{
    public async Task<List<Product>> GetAllAsync()
    {
        var products = await _db.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetById(int id)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }
}