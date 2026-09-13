namespace SalesDB.Controllers;

    using Microsoft.AspNetCore.Mvc;
using SalesDB.Services;

[Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            return await _service.GetAllOrdersAsync();
        }
        [HttpGet("{id}")]
        // ENDPOINT:
        // http://localhost:5035/api/Order......
        // from query ?
        // from route /         [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            
            return await _service.GetOrderByIdAsync(id);
        }
    }
