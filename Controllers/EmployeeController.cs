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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployee model)
        {
            return await _ser.AddAsync(model);
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