namespace SalesDB.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDB.Dtos;
using SalesDB.Services;

[Route("api/[controller]")]
[ApiController]
// chi co nguoi dung da xac thi thi moi xem dc ds product
[Authorize]
// chi cos admin moi xem dc
public class ProductController(IProductService _service) : ControllerBase
{
    [AllowAnonymous] // khong yeu cau token
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddProductRequest model)
    {
        return await _service.AddAsync(model);
    }
    [Authorize(Roles = "Admin")]

    [HttpPut("{id}")]
    public async Task<IActionResult> Post([FromBody] ProductDto model, int id)
    {
        return await _service.UpdateAsync(model, id);
    }
}
