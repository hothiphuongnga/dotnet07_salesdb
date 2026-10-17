// tạo bộ dùng chung cho all table in db 

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Entities;

namespace SalesDB.Repositories;

// TEntity là tên class 
// TKey là kiểu dữ liệu của khóa chính (primary key) của bảng
public interface IRepositoryBase<TEntity, TKey> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();

    // get byid
    // int , string 
    Task<TEntity?> GetByIdAsync(TKey id);

    Task<List<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);

    Task<List<TEntity>> SearchAsync(string fieldName, string keyword);
    Task<List<TEntity>> FindByFieldAsync(string fieldName, object? value);
    // Task<List<TEntity>> FindByMultipleFieldsAsync(Dictionary<string, object?> fieldValues);
    // Task<TEntity> CreateAsync(TEntity entity);
    // Task<TEntity> UpdateAsync(TEntity entity);
    // Task<bool> DeleteAsync(TKey id);
}

public class RepositoryBase<TEntity, TKey>(SalesDbContext _db) : IRepositoryBase<TEntity, TKey> where TEntity : class
{
    /// <summary>
    /// ddeer cac thag con implenent su dung
    /// </summary>
    protected readonly SalesDbContext Db = _db;
    private readonly DbSet<TEntity> _dbSet = _db.Set<TEntity>();


    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
    {
        // [1,1,1,2,5,4]
        var isList = ids.Distinct().ToList();
        // lấy tên 
        var keyName = Db.Model.FindEntityType(typeof(TEntity)); // Lấy thông tin EF entity
        // lấy key PF
        var primaryKey = keyName.FindPrimaryKey().Properties.First().Name;

        return await _dbSet.Where(e => isList.Contains(EF.Property<TKey>(e, primaryKey))).ToListAsync();
    }

    public Task<List<TEntity>> SearchAsync(string fieldName, string keyword)
    {
        // kiểm tra fieldName có trong TEntity hay không
        var property = GetProperty(fieldName);
        // I=> THI, PHI , NHI ,KIM 
        // viết câu query / viết SP, funtion view 

        // select * from TEntity where fieldName like N'%keyword%'
        return _dbSet.AsNoTracking().Where(e => EF.Property<string>(e, property.Name).Contains(keyword)).ToListAsync();
    }

    public Task<List<TEntity>> FindByFieldAsync(string fieldName, object? value)
    {
        // kiểm tra fieldName có trong TEntity hay không
        var property = GetProperty(fieldName);
        // VALUE = "NGA" => tìm tất cả các bản ghi có VALUE CỦA fieldName tương ứng bằng "NGA"
        // EF.Property : Lấy giá trị của một property trong entity theo tên property, hỗ trợ truy vấn động trong EF Core, 
        return _dbSet.AsNoTracking().Where(e => EF.Property<object?>(e, property.Name) == value).ToListAsync();
    }
    private PropertyInfo GetProperty(string fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
        {
            throw new ArgumentException("Tên field không được để trống.", nameof(fieldName));
        }
        return typeof(TEntity).GetProperties()
        .FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
         ??
         throw new ArgumentException($"Không tìm thấy field '{fieldName}' trong {typeof(TEntity).Name}.", nameof(fieldName));
    }

    // thống kê, cảnh báo ... 
}