namespace SalesDB.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using SalesDB.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomersService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _service.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        // api get ds bawnfg ds id

        [HttpGet("by-ids")]
        public async Task<IActionResult> GetByIds([FromQuery] int[] ids)
        {
            return await _service.GetByIdsAsync(ids);
        }
    }
}