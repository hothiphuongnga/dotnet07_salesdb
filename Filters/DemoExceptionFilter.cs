using Microsoft.AspNetCore.Mvc.Filters;
using SalesDB.Dtos.Base;

namespace SalesDB.Filters;

public class DemoExceptionFilter(ILogger<DemoExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogWarning("[4. EXCEPTION FILTER] Bỗi bắt từ filter");

        if (context.Exception is InvalidOperationException exception)
        {
            context.Result = new ResponseEntity(400, 
            new
            {
                Title = "Lỗi được xử lý bởi DemoExceptionFilter",
                Detail = exception.Message,
                Instance = context.HttpContext.Request.Path
            }, "Lỗi được xử lý bởi DemoExceptionFilter");
            // đánh dấu là đã xử lý lỗi để middle không xử lý lại
            context.ExceptionHandled = true;
            
        }
    }
}