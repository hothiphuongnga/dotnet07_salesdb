namespace SalesDB.Controllers
{
    using System.Text;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SalesDB.Dtos.Base;
    using SalesDB.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IRedisService _redis) : ControllerBase
    {
        [Authorize]
        [HttpGet("demo/{id}")]// demo/2
        public async Task<IActionResult> Get(int id, [FromHeader] string lang = "vi") // lang= vi , en, cn, jp
        {
            // HttpContext - quản lý toàn bộ thông tin của request và response
            var idParam = HttpContext.Request.RouteValues["id"]?.ToString();
            var langHeader = HttpContext.Request.Headers["lang"].ToString();
            // lấy i[ của client
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            // method
            var method = HttpContext.Request.Method;
            // path
            var path = HttpContext.Request.Path;

            // ?name=Nga&age=10
            var queryString = HttpContext.Request.QueryString;
            var nameQuery = HttpContext.Request.Query["name"].ToString(); // nga

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🟢 Demo");
            Console.WriteLine($"🟢 IdParam:                 {idParam}");
            Console.WriteLine($"🟢 Lang:                    {lang}");
            Console.WriteLine($"🟢 ClientIP:                {clientIp}");
            Console.WriteLine($"🟢 Method:                  {method}");
            Console.WriteLine($"🟢 Path:                    {path}");
            Console.WriteLine($"🟢 Query String:            {queryString}"); //?name=Nga
            Console.WriteLine($"🟢 Query Name:              {nameQuery}"); //Nga

            Console.ResetColor();



            // return kết quả 
            // Trả kết quả cho client
            return Ok(new
            {
                Message = "Demo HttpContext thành công!",
                Id = idParam,
                Lang = langHeader,
                ClientIp = clientIp
            });

        }
        [HttpPost("demoBody")]
        public async Task<IActionResult> DemoPost() // 
        {
            HttpContext.Request.EnableBuffering();

            // {"name":"Nga","age":25}

            using var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, true);
            var bodyString = await reader.ReadToEndAsync();
            HttpContext.Request.Body.Position = 0;
            Console.WriteLine($"🟢 Body:              {bodyString}");


            return Ok(new { Message = "Đã đọc body thành công" });

        }
        [HttpGet("exception")]
        public async Task<ActionResult<string>> GetException(int a)
        {
            // int res = 20 / a;
            // return Ok(res);
            // chủ động trả lỗi
            throw new KeyNotFoundException("Loi 404 tu tao de test");
        }

        [HttpGet("demoRedis")]
        public async Task<IActionResult> DemoRedis(string key, string value, int minutes)
        {

            var res = await _redis.SetAsync(key, value, TimeSpan.FromMinutes(minutes));
            var result = new ResponseEntity(200, res, "OK");
            return result;
        }
        [HttpGet("getRedis")]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _redis.GetAsync(key);
            var result = new ResponseEntity(200, value, "OK");
            return result;
        }

    }
}