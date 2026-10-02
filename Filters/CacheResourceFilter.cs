using Microsoft.AspNetCore.Mvc.Filters;
using SalesDB.Dtos.Base;

namespace SalesDB.Filters;

public class CacheResourceFilter(ILogger<CacheResourceFilter> logger) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // xử lý cache
        // demo-filter/cache?fromCache=true -> lấy dữ liệu từ server cache
        // fromCache=false / không có thì lấy dữu lieu tu hệ thống thông qua action method
        // lấy fromCache bằng context
         // BEFORE: chạy trước model binding, action filter và action method.
        logger.LogInformation("[2. RESOURCE FILTER - BEFORE] Chưa thực hiện action method");
        var cache = context.HttpContext.Request.Query["fromCache"];
        if(cache == "true")
        {
            // lấy từ server cache
            // khỏi chạy action method, => model biding không chạy
            context.Result = new ResponseEntity(200, true,"Kết quả giả lập lấy từ cache trong Resource Filter");

            return;
        }
        // nêu không có cach thì đi tiếp
        // chạy action method
        var ex = await next();

        // chạy excuted sau dòng next()
        // chạy action method xong sẽ quay lại chỗ này thực hiện excuted
         // AFTER: chạy khi action/result pipeline đã quay trở ra.
        logger.LogInformation(
            "[2. RESOURCE FILTER - AFTER] Canceled: {Canceled}, Exception: {HasException}",
            ex.Canceled,
            ex.Exception is not null);



    }
}