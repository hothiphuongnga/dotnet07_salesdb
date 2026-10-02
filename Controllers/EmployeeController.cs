namespace SalesDB.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SalesDB.Dtos;
    using SalesDB.Dtos.Base;
    using SalesDB.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _ser) : ControllerBase
    {
        //
        [HttpPost("register")]
        // xử lý model biding trước khi đi vào thực thi nội dung hàm
        public async Task<IActionResult> Register([FromBody] RegisterEmployee model)
        {
            // action filter before

            // action method
            return await _ser.AddAsync(model);
            
            // action filter after

            // 
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            return await _ser.LoginAsync(model);
        }

        // cần token để lấy profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            // lấy ra ID ở trong token
            var employeeClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // kiểm tra trước khi gọi service
            if(!int.TryParse(employeeClaim, out var employeeId))
            {
                return new ResponseEntity(401, null,"Token không hợp lệ");
            }
            return await _ser.GetProfileById(employeeId);

        }
    }
    
}