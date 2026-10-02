using Microsoft.AspNetCore.Mvc.Filters;

namespace SalesDB.Filters;

// Xuwr ly log dong bo
public class DemoResourceFilter : IResourceFilter
{
    private string path  ="logs/log.txt";
    public DemoResourceFilter()
    {
        string? logdir = Path.GetDirectoryName(path);
        if (!Directory.Exists(logdir))
        {
            Directory.CreateDirectory(logdir);
        }
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        // SAU AUTH FILTER
        // TRƯỚC ACTION METHOD
        string logStart = @$"[Resource - Executing] 
        - Request : {context.HttpContext.Request.Method} 
        - Path: {context.HttpContext.Request.Path}
        - Datetime: {DateTime.Now}\n";
        File.AppendAllText(path, logStart);
    }
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // SAU ACTION METHOD
        string logEnd = @$"[Resource - Executed] 
        - Request : {context.HttpContext.Request.Method} 
        - Path: {context.HttpContext.Request.Path}
        - Datetime: {DateTime.Now}\n";
        File.AppendAllText(path, logEnd);
    }
}

