using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IProductService
{
    // getall , getbyid
    Task<ResponseEntity> GetAllProductsAsync();
    Task<ResponseEntity> GetProductByIdAsync(int id);

}
public class ProductService(IProductRepository _repository) : IProductService
{
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
        var product = await _repository.GetById(id);
        var res = new ProductDto()
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price,
            Stock = product.Stock
        };
        return new ResponseEntity(200, res, "Lay san pham thanh cong");
    }
}

// AI => auto in
// 