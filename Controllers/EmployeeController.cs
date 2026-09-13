namespace SalesDB.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using SalesDB.Dtos;
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
    }
    
}