using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SalesDB.Data;

namespace SalesDB.Repositories;
//  UOW (Unit of Work) pattern  quản lý nhiềurepo cùng lúc
//  đảm bảo tất cả các thay đổi trong một transaction duy nhất, dễ rollback
// gom  savechange 


public interface IUnitOfWork : IDisposable
{
    // LIẸT KÊ CÁC REPO CẦN QUẢN LÝ
    // lazy innit
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IEmployeeRepository  EmployeeRepository{ get; }

    /// <summary>
    /// Lưu tất cả các thay đổi trong một transaction duy nhất
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();

    // begin transaction
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}


public class UnitOfWork: IUnitOfWork
{
    private readonly SalesDbContext _db;
    private IDbContextTransaction _transaction;
    private IOrderRepository _orderRepository;
    private IProductRepository _productRepository;
    private ICustomerRepository _customerRepository;
    private IEmployeeRepository _employeeRepository;
    
    

    public UnitOfWork(SalesDbContext db)
    {
        _db = db;
    }


    public IOrderRepository OrderRepository
    {
        get { 
            _orderRepository = new OrderRepository(_db);
            return _orderRepository;
        }
    }

    public IProductRepository ProductRepository
    {
        get
        {
            _productRepository = new ProductRepository(_db);
            return _productRepository;
        }
    }

    public ICustomerRepository CustomerRepository
    {
        get
        {
            _customerRepository = new CustomerRepository(_db);
            return _customerRepository;
        }
    }

    public IEmployeeRepository EmployeeRepository
    {
        get
        {
            _employeeRepository = new EmployeeRepository(_db);
            return _employeeRepository;
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _db.Database.BeginTransactionAsync();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[UOW] ► Bắt đầu transaction");
        Console.ResetColor();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction.CommitAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[UOW] ► Commit transaction thành công");
            Console.ResetColor();
        }
        catch(Exception ex)
        {
            await RollbackTransactionAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[UOW ERROR] Commit thất bại: {ex.Message}");
            Console.ResetColor();
            throw;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _db?.Dispose();
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[UOW] ↺ Rollback Transaction");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[UOW ERROR] Rollback thất bại: {ex.Message}");
            Console.ResetColor();
            throw;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[UOW ERROR] SaveChanges thất bại: {ex.Message}");
            Console.ResetColor();
            throw;
        }
    }
}