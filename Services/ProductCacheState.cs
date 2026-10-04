using Microsoft.Extensions.Primitives;

namespace SalesDB.Services;

public class ProductCacheState
{
    private CancellationTokenSource _cts = new();
    public IChangeToken GetToken()
    {
        return new CancellationChangeToken(_cts.Token);
    }   
    public void Clear()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("-------[CLEAR CACHE]-------");
        Console.ResetColor();
        _cts.Cancel();
        // tạo token mới cho lần cache sau

        _cts.Dispose();
        _cts = new CancellationTokenSource();
    }
}