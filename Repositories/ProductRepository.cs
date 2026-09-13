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
    Task<Product> AddAsync(Product model);
    Task<Product> UpdateAsync(Product model);


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

    // post vaf put

    public async Task<Product> AddAsync(Product model)
    {
        await _db.Products.AddAsync(model);
        // tao ra cau sql
        await _db.SaveChangesAsync();
        return model;

    }
    public async Task<Product> UpdateAsync(Product model)
    {
        _db.Products.Update(model);
        // tao ra cau sql
        await _db.SaveChangesAsync();
        return model;
    }
}