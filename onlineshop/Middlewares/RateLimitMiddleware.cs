using Microsoft.Extensions.Caching.Memory;
using onlineshop.Exceptions;

namespace onlineshop.Middlewares;

public class RateLimitMiddleware(RequestDelegate next, IMemoryCache memoryCache)
{
    private readonly TimeSpan timeLimit = TimeSpan.FromMinutes(1);
    private readonly int countLimit = 1000;

    public async Task Invoke(HttpContext context)
    {
        var key = context.Connection.RemoteIpAddress!.ToString();

        memoryCache.TryGetValue(key, out int requestCount);

        if (requestCount > countLimit)
        {
            throw new TooManyRequestException("Too many request");
        }
        else
        {
            await next(context);

            requestCount++;
            memoryCache.Set(key, requestCount, timeLimit);
        }
    }
}
