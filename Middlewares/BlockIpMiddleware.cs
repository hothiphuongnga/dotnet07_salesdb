using System.Diagnostics;
using System.Net;

namespace SalesDB.Middlewares;

// xử lý tổng thời gian request từ lúc bắt đầu đến khi nhận được response và kết thúc
public class BlockIpMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 
        var clienIp = context.Connection.RemoteIpAddress;
                Console.WriteLine("🟢 [BLOCK] - " + clienIp);

        string[] blockIps = [""];
        // ddooir ::1 thanh ip thucwj te can lock

        var isBlock = clienIp is not null && blockIps.Any(value => IPAddress.TryParse(value, out var blockIp) && AreEqual(clienIp, blockIp));
        if (!isBlock)
        {
            await next(context);
            return;
        }
        Console.WriteLine("🟢 [END-BLOCK] - " + clienIp);

    }
    private static bool AreEqual(IPAddress fisrt, IPAddress second)
    {
        if (fisrt.IsIPv4MappedToIPv6)
        {
            fisrt = fisrt.MapToIPv4();
        }
        if (second.IsIPv4MappedToIPv6)
        {
            second = second.MapToIPv4();
        }
        return fisrt.Equals(second);
    }
}
