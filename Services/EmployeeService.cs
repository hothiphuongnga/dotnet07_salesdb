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
    Task<IActionResult> AddAsync(RegisterEmployee model); 
    Task<IActionResult> LoginAsync(LoginRequest model);
    Task<IActionResult> GetProfileById(int id);

}

// DIP
public class EmployeeService(
    IEmployeeRepository _repo, 
    IMapper _mapper,
    IJwtService _jwt) : IEmployeeService
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
        // login thành công sẽ trả ra token , Name, id, role => tạo class (DTO)
        // đổi từ Employee -> LoginResponse
        var response = _mapper.Map<LoginResponse>(emp);
        response.Token = _jwt.GenerateToken(emp);
        if (isPasswordValid)
        {
            return new ResponseEntity(200, response, "Login succes");
        }

        return new ResponseEntity(401, new { }, "Số điện thoại hoặc mật khẩu không đúng");
        // CLAIM - ID , Name, Role, 
        // jwt: 
        // cccd , luong , () token - xác thực danh tính

        // AUTHEN: XÁC THỰC     1 CODE 401
        // AUTHOR: PHÂN QUYỀN   2 CODE 403

    }
    public async Task<IActionResult> GetProfileById(int id)
    {
        var emp = await _repo.GetByIdAsync(id);
        var res  = _mapper.Map<EmployeeDto>(emp);
        return new ResponseEntity(200,res,"Get profile success");
    }
}