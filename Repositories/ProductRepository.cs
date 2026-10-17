using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

// ket noi den DB , 
// su dung EF 
public interface IProductRepository : IRepositoryBase<Product, int>
{
    // GetAll
    // GetById
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetById(int id);
    Task<Product> AddAsync(Product model);
    Task<Product> UpdateAsync(Product model);
    Task<List<Product>> GetPaging(int page, int size, string? keywords);
    Task<int> CountPaging(int page, int size, string? keywords);


}
public class ProductRepository(SalesDbContext _db) :
    RepositoryBase<Product, int>(_db),
 IProductRepository
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
    public async Task<List<Product>> GetPaging(int page, int size, string? keywords)
    {
        // 
        int offset = (page - 1) * size;
        string sql = @"SELECT * FROM Products Where ProductName 
            like N'%'+ @Keywords + '%' ORDER  by id desc 
            offset @Offset rows fetch next @Size rows only";

        var res = await _db.Products.FromSqlRaw( // .Products laf EF hoox troj tra ra List<Product?
            sql,
            new SqlParameter("@Keywords", keywords),
            new SqlParameter("@Offset", offset),
            new SqlParameter("@Size", size)).AsNoTracking()
            .ToListAsync();

        return res;

    }
    public async Task<int> CountPaging(int page, int size, string? keywords)
    {
        string sql = @"SELECT count(*) as value FROM Products Where ProductName 
            like N'%'+ @Keywords + '%'";

        var res = await _db.Database.SqlQueryRaw<int>(
            sql,
            new SqlParameter("@Keywords", keywords))
            .SingleAsync();

        return res;
    }




}