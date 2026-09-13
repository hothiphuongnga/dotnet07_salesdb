namespace SalesDB.Controllers;

using Microsoft.AspNetCore.Mvc;
using SalesDB.Dtos;
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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddProductRequest model)
    {
        return await _service.AddAsync(model);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Post([FromBody] ProductDto model, int id)
    {
        return await _service.UpdateAsync(model, id);
    }
}
