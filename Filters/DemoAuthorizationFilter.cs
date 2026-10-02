//  chỉ cho phép api có key là net-07 truy cập
// if(studentKey != 'net07')
// {
//     throw new ArgumentException("", "");
// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SalesDB.Dtos.Base;

namespace SalesDB.Filters;

public class DemoAuthorizationFilter(ILogger<DemoAuthorizationFilter> logger) : IAsyncAuthorizationFilter
{
    // Vis dụ về cơ chế filter , trong thực tế thì xác thực phân quyền vẫn nên dùng Middleware
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        logger.LogInformation("[1. AUTHORIZATION FILTER] Kiểm tra quyền trước các filter khác");

        // kiểm tra trong header có key tên X-Student-Key == 'NET07'
        var studentKey = context.HttpContext.Request.Headers["X-Student-Key"].ToString();
        if (studentKey != "NET07")
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseEntity(401, "Thêm header: X-Student-Key: NET07", "Thiếu hoặc sai header X-Student-Key")
            );
        }
        return Task.CompletedTask;
    }
}
// FE cấu hình interceptor  trong axios 
// 