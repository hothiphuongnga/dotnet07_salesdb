using AutoMapper;
using SalesDB.Dtos.Base;
using SalesDB.Dtos.Customer;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface ICustomersService
{
    Task<ResponseEntity> GetAllAsync();
    Task<ResponseEntity> GetByIdAsync(int id);
}

public class CustomersService(ICustomerRepository _repository, IMapper _mapper) : ICustomersService
{
    public async Task<ResponseEntity> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        // var res = customers.Select(c => new CustomerResponse()
        // {
        //     Id = c.Id,
        //     Name = c.Name,
        //     Email = c.Email,
        //     Phone = c.Phone,
        //     Address = c.Address
        // });
        var res = _mapper.Map<List<CustomerResponse>>(customers);
        return new ResponseEntity(200, res, "");
    }

    public async Task<ResponseEntity> GetByIdAsync(int id)
    {
        var customer = await _repository.GetById(id);
        // var res = new CustomerResponse()
        // {
        //     Id = customer.Id,
        //     Name = customer.Name,
        //     Email = customer.Email,
        //     Phone = customer.Phone,
        //     Address = customer.Address
        // };
        var res = _mapper.Map<CustomerResponse>(customer);

        return new ResponseEntity(200, res, "");
    }
}