using AutoMapper;
using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IOrderService
{
    // getall , getbyid
    Task<ResponseEntity> GetAllOrdersAsync();
    Task<ResponseEntity> GetOrderByIdAsync(int id);

}

public class OrderService(IOrderRepository _repo, IMapper _mapper) : IOrderService
{
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