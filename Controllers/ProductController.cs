namespace SalesDB.Controllers;

using Microsoft.AspNetCore.Mvc;
using SalesDB.Services;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = await _service.GetAllProductsAsync();
        return res;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var res = await _service.GetProductByIdAsync(id);
        return res; 
    }
}
