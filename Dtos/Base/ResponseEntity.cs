using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SalesDB.Dtos.Base;

public class ResponseEntity : IActionResult
{
    public int StatusCode { get; set; }
    public object Content { get; set; }
    public string Message { get; set; }
    public DateTime Datetime { get; set; } = DateTime.Now;
    public ResponseEntity(int statusCode, object content, string message = "")
    {
        StatusCode = statusCode;
        Content = content;
        Message = message;
    }

    public async Task ExecuteResultAsync
    (ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = StatusCode;
        response.ContentType = "application/json";

        var payload = new
        {
            statusCode = StatusCode,
            message = Message,
            content = Content,
            dateTime = DateTime.Now,
        };
        // Serialize ra JSON (đảm bảo dùng UTF-8, ignore nulls)
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true,
            // MaLop => maLop
            // TenLop => tenLop....
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            // DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        await response.WriteAsync(json);

    }
}