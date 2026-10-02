using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using SalesDB.Dtos.Base;

namespace SalesDB.Filters;

public class DemoActionFilter(ILogger<DemoActionFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        logger.LogInformation("[3. ACTION FILTER - BEFORE] Action: {Action}; Argument: {@Arguments}", context.ActionDescriptor.DisplayName, context.ActionArguments);
        // vì model biding đã chạy rồi
        // 
        if (context.ActionArguments.TryGetValue("id", out var value) && value is int id && id <= 0)
        {
            context.Result = new ResponseEntity(400, null, "Action filter yêu cầu id là số phải lớn hơn 0");
            return;
        }
        // bấm giờ trước khi thực thi
        var stopwatch = Stopwatch.StartNew();
        var ex = await next();

        // 
        stopwatch.Stop();
        logger.LogInformation("[3. ACTION FILTER - AFTER] Action chạy {Elapsed} ms; Exception: {HasException}", stopwatch.ElapsedMilliseconds, ex.Exception is not null);
    }
}