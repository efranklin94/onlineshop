using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace onlineshop.Middlewares;

public class IdempotencyMiddleware(RequestDelegate next, IMemoryCache memoryCache) 
{
    public async Task Invoke(HttpContext context)
    {
        var IpAddress = context.Connection.RemoteIpAddress?.ToString();
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var query = context.Request.QueryString.HasValue? context.Request.QueryString.Value : string.Empty;
        var combinedKey = $"{IpAddress}-{method}-{path}-{query}";
        var cacheKey = ComputeMD5Hash(combinedKey);

        if (memoryCache.TryGetValue(cacheKey, out var cachedObject))
        {
            Console.WriteLine($"{JsonSerializer.Serialize(cachedObject)} is fetched from the idempotent cache");

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(cachedObject));

            return;
        }

        await next(context);

        if (context.Items.TryGetValue("IdempotencyResponse", out var result))
        {
            Console.WriteLine($"{result?.ToString()} is wrote to the idempotent cache");

            memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
        }
    }

    private static string ComputeMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert hash bytes to a hex string
            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}
