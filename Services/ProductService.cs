using System.Text.Json;
using AutoMapper;
using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Entities;
using SalesDB.Repositories;
using StackExchange.Redis;

namespace SalesDB.Services;

public interface IProductService
{
    // getall , getbyid
    Task<ResponseEntity> GetAllProductsAsync();
    Task<ResponseEntity> GetProductByIdAsync(int id);
    Task<ResponseEntity> AddAsync(AddProductRequest model);
    Task<ResponseEntity> UpdateAsync(ProductDto model, int id);
    Task<ResponseEntity> GetPaging(int page, int size, string? keywords);
}
public class ProductService(
    IProductRepository _repository,
    IMapper _mapper,
    ProductCacheState _productCacheState,
    // IConnectionMultiplexer _redis,
    IRedisService _redisService) : IProductService
{
    // lấy DB từ redis
    // private readonly IDatabase _db = _redis.GetDatabase();
    public async Task<ResponseEntity> AddAsync(AddProductRequest model)
    {
        // clear cache product()
        // service -> repo
        // thuc hien chuyen doi tu AddProductRequest -> Product
        var obj = _mapper.Map<Product>(model);
        var res = await _repository.AddAsync(obj);
        _productCacheState.Clear();
        return new ResponseEntity(201, _mapper.Map<ProductDto>(res), "add success");
    }

    public async Task<ResponseEntity> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        var res = products.Select(p => new ProductDto()
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Price = p.Price,
            Stock = p.Stock
        }).ToList();
        return new ResponseEntity(200, res, "Lay ds san pham thanh cong");
    }

    public async Task<ResponseEntity> GetProductByIdAsync(int id)
    {
        string key = $"saledb:product:{id}";
        // kiểm tra cache redis trước
        var cached = await _redisService.GetAsync(key);
        // cache HIT
        if (string.IsNullOrEmpty(cached))
        {
            Console.WriteLine("[HIT] 👊 lấy dữ liệu trên redis");
            var dto = JsonSerializer.Deserialize<ProductDto>(cached.ToString());
            if (dto != null)
            {
                return new ResponseEntity(200, dto, "Lay san pham thanh cong");
            }
        }
        // cache MISS
        Console.WriteLine("[HIT] ⚠ lấy dữ liệu từ DB");

        var product = await _repository.GetById(id);
        var res = new ProductDto()
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price,
            Stock = product.Stock
        };
        await _redisService.SetAsync(key, JsonSerializer.Serialize(res),TimeSpan.FromMinutes(5));
        return new ResponseEntity(200, res, "Lay san pham thanh cong");
    }

    public async Task<ResponseEntity> UpdateAsync(ProductDto model, int id)
    {
        // clear cache khi sp thay đổi
        // check ton tai  -> update
        var product = await _repository.GetById(id);

        //
        if (product is null)
        {
            return new ResponseEntity(
               404,
               new { },
               "Không tìm thấy sản phẩm");
        }
        // map du lieu cua model vap cho product
        _mapper.Map(model, product);
        var res = await _repository.UpdateAsync(product);
        _productCacheState.Clear(); // clear in-memory cache
        // CLEAR CACHE trên REDIS
        await _redisService.RemoveAsync($"saledb:product:{id}");
        

        return new ResponseEntity(200, _mapper.Map<ProductDto>(res), "update success");
    }

    public async Task<ResponseEntity> GetPaging(int page, int size, string? keywords)
    {
        var res = await _repository.GetPaging(page, size, keywords);
        var totalItems = await _repository.CountPaging(page, size, keywords);

        var items = _mapper.Map<List<ProductDto>>(res);
        // size =10 , totalItems=10 => 1 page 
        // size = 10 , totalItems= 11 => 2 page  11/10 => 1,0001
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);
        // (totalItems + size -1 )/ size
        // 10 + 10 -1 => 19 / size  = 1
        // 11 + 10 -1 => 20 / 10 = 2

        var paging = new PagingResult<ProductDto>
        {
            Items = items,
            TotalItems = totalItems,
            TotalPages = totalPages,
            PageIndex = page,
            PageSize = size
        };

        return new ResponseEntity(200, paging, $"Trang {page} với {size} product");
    }








}

// AI => auto in
// 