using StackExchange.Redis;

namespace SalesDB.Services;

public interface IRedisService
{
    // set redis
    // nhận key và value , thời gian tồn tại của cache : có thể truyền hoặc không
    Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);

    // getredis
    Task<string?> GetAsync(string key);
}


public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        var exp = expiry.HasValue ? new Expiration(expiry.Value) : Expiration.Default;

        return  await _db.StringSetAsync(key, value,exp);
    }
    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    
}