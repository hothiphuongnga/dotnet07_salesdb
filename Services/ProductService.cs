using AutoMapper;
using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Entities;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IProductService
{
    // getall , getbyid
    Task<ResponseEntity> GetAllProductsAsync();
    Task<ResponseEntity> GetProductByIdAsync(int id);
    Task<ResponseEntity> AddAsync(AddProductRequest model);
    Task<ResponseEntity> UpdateAsync(ProductDto model, int id);


}
public class ProductService(IProductRepository _repository, IMapper _mapper) : IProductService
{
    public async Task<ResponseEntity> AddAsync(AddProductRequest model)
    {

        // service -> repo
        // thuc hien chuyen doi tu AddProductRequest -> Product
        var obj = _mapper.Map<Product>(model);
        var res = await _repository.AddAsync(obj);

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

    public async Task<ResponseEntity> UpdateAsync(ProductDto model, int id)
    {
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
        return new ResponseEntity(200, _mapper.Map<ProductDto>(res), "update success");
    }
}

// AI => auto in
// 