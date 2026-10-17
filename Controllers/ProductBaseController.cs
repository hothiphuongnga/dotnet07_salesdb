namespace SalesDB.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using SalesDB.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductBaseController(IProductService _ser) : ControllerBase
    {
        // sử dụng các hàm của có trong base 
        // cho service : IServiceBase
        // cho repo : IRepositoryBase
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _ser.GetAllAsync();
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return await _ser.GetByIdAsync(id);
        }
        [HttpGet("by-ids")]
        public async Task<IActionResult> GetByIdsAsync([FromQuery] List<int> ids)
        {
            return await _ser.GetByIdsAsync(ids);
        }

        // Ví dụ: GET /api/ProductBase/search?fieldName=ProductName&keyword=laptop
        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] string fieldName,
            [FromQuery] string keyword)
        {
            return await _ser.SearchAsync(fieldName, keyword);
        }

        // Ví dụ: GET /api/ProductBase/find?fieldName=Stock&value=10
        [HttpGet("find")]
        public async Task<IActionResult> FindByFieldAsync(
            [FromQuery] string fieldName,
            [FromQuery] string value)
        {
            return await _ser.FindAsync(fieldName, value);
        }

    }
}