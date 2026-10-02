namespace SalesDB.Controllers;

using Microsoft.AspNetCore.Mvc;
using SalesDB.Dtos.Base;
using SalesDB.Filters;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(DemoResourceFilter))]
[ServiceFilter(typeof(CacheResourceFilter))]
[ServiceFilter(typeof(DemoActionFilter))]
[ServiceFilter(typeof(DemoExceptionFilter))]
[ServiceFilter(typeof(DemoResultFilter))]

public class FilterDemoController : ControllerBase
{

    [HttpGet("protected")]
    [ServiceFilter(typeof(DemoAuthorizationFilter))]

    public async Task<IActionResult> Get()
    {
        return new ResponseEntity(200, null, "Authorization Filter đã cho phép truy cập");
    }
    [HttpGet("cache")]
    public async Task<IActionResult> Cache(string fromCache)
    {
        return new ResponseEntity(200, fromCache, "Không lấy cách nên action method chạy");
    }

    // GET /api/filter-demo/lifecycle/10
    // Thử id = 0 để Action Filter short-circuit trước khi action này chạy.
    [HttpGet("lifecycle/{id:int}")]
    public IActionResult GetLifecycle(int id)
    {
        return new ResponseEntity(200, null,"Action method đã thực thi");
    }
    // api test model biding khi đã áp dụng action filter
    [HttpPost("model-binding")]
    public IActionResult PostAsync([FromBody] FilterStudentRequest model)
    {
        return new ResponseEntity(200, model,"Model binding thành công");
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        throw new InvalidOperationException("Lỗi chủ động để test filter");
    }
}
public record FilterStudentRequest(string Name, int Age);
