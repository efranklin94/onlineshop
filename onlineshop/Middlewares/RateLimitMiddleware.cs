using DomainService;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using onlineshop.Exceptions;

namespace onlineshop.Middlewares;

public class RateLimitMiddleware(RequestDelegate next, IMemoryCache memoryCache, ICurrentUser currentUser)
{
    private readonly TimeSpan timeLimit = TimeSpan.FromMinutes(1);
    private readonly int countLimit = 1000;

    public async Task Invoke(HttpContext context)
    {
        var key = currentUser.IPAddress.ToString();

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
