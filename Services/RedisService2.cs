using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace SalesDB.Services;

public interface IRedisService2
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task DeleteAsync(string key);
}


public class RedisService2(IDistributedCache _cache, ILogger<RedisService2> _log) : IRedisService2
{
    public async Task DeleteAsync(string key)
    { 
        await _cache.RemoveAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var cache = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cache)) // không có giá trị 
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(cache);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);
        var option = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
        {
            option.AbsoluteExpirationRelativeToNow = expiration.Value;
        }
        await _cache.SetStringAsync(key, json, option);
    }
}


// cache đơn giản thì dungf redisservice 2,
//  không cần cấu hình phức tạp như redisservice