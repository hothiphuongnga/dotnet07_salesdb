using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using SalesDB.Services;

namespace SalesDB.Filters;

public class SimpleCacheFilter(
    ILogger<SimpleCacheFilter> _logger,
    IMemoryCache _cache,
    ProductCacheState _productCacheState

    ) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // TRƯỚC ACTION METHOD
        // lấy ra ds product trong db
        // kiểm tra xem là thông tin của pdoduct có trong cache hay chưa , nếu lưu rồi thì lấy từ trong memory ra , không cần thực thi action method
        // nếu không có => cho chạy action method
        // demo/products?keyword=adidas

        var key = context.HttpContext.Request.Path.ToString()
                  + context.HttpContext.Request.QueryString.ToString();
        // kiểm tra xem key này có trong cache hay chưa
        if (_cache.TryGetValue(key, out IActionResult? cachedResponse))
        {
            // neu co trong cache - CACHE HIT
            context.Result = cachedResponse;
            _logger.LogInformation("[Resource Filter] Trả về kết quả từ cache cho key: " + key);
            return;
        }
        // khong co trong cache - CACHE MISS
        _logger.LogWarning("[Resource Filter] Cache MISS: " + key);
        // CHAY action method
        var executedContext = await next();
        // SAU ACTION METHOD
        // chạy xong thì nhận dc dữ liêun product
        // nếu có kết quả đúng thì mới cache
        // yêu cầu bị cancel , , bị lỗi, response null
        if (executedContext.Canceled || executedContext.Exception != null || executedContext.Result == null)
        {
            return;
        }
        var cacheOption = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        // taoj token chung cho tat ca product
        if (context.HttpContext.Request.Path.StartsWithSegments("/api/Product", StringComparison.OrdinalIgnoreCase))
        {
            cacheOption = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .AddExpirationToken(_productCacheState.GetToken());
        }


        // lưu cache cho lần sau có mà dùng
        _cache.Set(key, executedContext.Result, cacheOption);
    }
}