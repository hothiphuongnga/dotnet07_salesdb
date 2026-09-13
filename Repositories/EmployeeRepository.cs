using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;


public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee model);
    Task<Employee?> FindByPhoneAsync(string phone);

}
public class EmployeeRepository(SalesDbContext _db) : IEmployeeRepository
{
    public async Task<Employee> AddAsync(Employee model)
    {
        await _db.AddAsync(model);
        await _db.SaveChangesAsync();
        return model;
    }

    public Task<Employee?> FindByPhoneAsync(string phone)
    {
        return _db.Employees.FirstOrDefaultAsync(e => e.Phone == phone);
    }
}

