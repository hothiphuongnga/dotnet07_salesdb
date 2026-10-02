using Microsoft.AspNetCore.Mvc.Filters;

namespace SalesDB.Filters;

public class DemoResultFilter(ILogger<DemoResultFilter> logger) : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {

        // chuẩn bị trả 
        logger.LogInformation("[5. RESULT FILTER - BEFORE] Đang chuẩn bị thực thi result");
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
        // trả xong rồi
        // 
        logger.LogInformation("[5. RESULT FILTER - AFTER] Đã thực thi result");

    }



}