using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetById(int id);
}
public class CustomerRepository(SalesDbContext _db) : ICustomerRepository
{
    public async Task<List<Customer>> GetAllAsync()
    {
        var customers = await _db.Customers.ToListAsync();
        return customers;
    }

    public async Task<Customer?> GetById(int id)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(p => p.Id == id);
        return customer;
    }
}