using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using SalesDB.Dtos;
using SalesDB.Dtos.Base;
using SalesDB.Entities;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IEmployeeService
{
    Task<IActionResult> AddAsync(RegisterEmployee model); Task<IActionResult> LoginAsync(LoginRequest model);

}

public class EmployeeService(IEmployeeRepository _repo, IMapper _mapper) : IEmployeeService
{
    public async Task<IActionResult> AddAsync(RegisterEmployee model)
    {
        var emp = _mapper.Map<Employee>(model);
        emp.Password = BCrypt.Net.BCrypt.HashPassword(emp.Password, workFactor: 12);

        emp = await _repo.AddAsync(emp);
        var res = _mapper.Map<EmployeeDto>(emp);
        return new ResponseEntity(201, res, "register success");
    }

    public async Task<IActionResult> LoginAsync(LoginRequest model)
    {
        // phone, mk
        // tim xem co  tk hay khong
        var emp = await _repo.FindByPhoneAsync(model.Phone);
        if (emp is null || string.IsNullOrWhiteSpace(emp.Password))
        {
            return new ResponseEntity(401, new { }, "Số điện thoại hoặc mật khẩu không đúng");
        }
        // kiem tra pass
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, emp.Password);

        //
        if (isPasswordValid)
        {
            return new ResponseEntity(200, emp, "Login succes");
        }

        return new ResponseEntity(401, new { }, "Số điện thoại hoặc mật khẩu không đúng");


    }
}