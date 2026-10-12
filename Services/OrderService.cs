using AutoMapper;
using SalesDB.Data;
using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Entities;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IOrderService
{
    // getall , getbyid
    Task<ResponseEntity> GetAllOrdersAsync();
    Task<ResponseEntity> GetOrderByIdAsync(int id);
    Task<ResponseEntity> CreateAsync(CreateOrderDto createOrderDto);

}

public class OrderService(IOrderRepository _repo, IMapper _mapper, SalesDbContext _context) : IOrderService
{
    public async Task<ResponseEntity> CreateAsync(CreateOrderDto createOrderDto)
    {
        // kieerm tra customer ID, 
        // kiem tra so luong san pham trong order
        // kiem  tra trung id trong ds san pham
        // lay ds sp ra de ktra tom kho  ds Id
        var productIds = createOrderDto.Items.Select(i => i.ProductId);
        var products = await _repo.GetProductsAsync(productIds);
        // TRANSACTION
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // createOrderDto -> Order
            Order order = new Order()
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };
            // chayj vong lap xu ly chi tiet don hang
            foreach (var item in createOrderDto.Items)
            {
                // kiem tra san pham cos du ton kho hay khong
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"khong du ton kho {product.ProductName}");
                }
                product.Stock -= item.Quantity; // giarm toomf kho 
                                                // taoj orrder detail
                order.OrderDetails.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
                // chuaarn xong order 
            }
            order.TotalAmount = order.OrderDetails.Sum(od => od.Quantity * od.Price);

            await _repo.CreateAsync(order);
            // demo looix 
            // throw new Exception("Demo loi - loi thanh toan");
            await transaction.CommitAsync();
            return new ResponseEntity(200, _mapper.Map<OrderDto>(order), "Order created successfully");


        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }






    }

    public async Task<ResponseEntity> GetAllOrdersAsync()
    {
        var orders = await _repo.GetAllAsync();
        var res = _mapper.Map<List<OrderDto>>(orders);
        return new ResponseEntity(200, res, "");
    }

    public async Task<ResponseEntity> GetOrderByIdAsync(int id)
    {
        var order = await _repo.GetByIdAsync(id);
        var res = _mapper.Map<OrderDto>(order);
        return new ResponseEntity(200, res, "");
    }
}