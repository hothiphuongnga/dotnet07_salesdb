using AutoMapper;
using SalesDB.Dtos;
using SalesDB.Dtos.Customer;
using SalesDB.Entities;

namespace SalesDB.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //  Dng ky , set uop cac thong tin de mapping du lieu ph hop
        // map cac fied trung ten lai voi nhau
        CreateMap<Customer, CustomerResponse>();
        CreateMap<CustomerResponse, Customer>();
        // <Nguon, ketqua>


        // cot trong db fdHoTen -> Hoten

        // date trong OrderDto se = orderdate trong Order
        CreateMap<Order, OrderDto>()
        .ForMember(dest => dest.Date, opt=> opt.MapFrom(src => src.OrderDate));
        //dest => dest.Date đích/ kết quả
        // src : nguồn

        CreateMap<Product, AddProductRequest>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        
        CreateMap<Employee,RegisterEmployee>().ReverseMap();
        CreateMap<Employee,EmployeeDto>().ReverseMap();
        // map emloy và loginresponse
        CreateMap<Employee, LoginResponse>().
        ForMember(dest => dest.Token, opt=> opt.Ignore());



    }
}