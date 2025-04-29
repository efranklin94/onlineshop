using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static onlineshop.Middlewares.IdempotencyMiddleware;

namespace onlineshop.Middlewares;

public class IdempotencyMiddleware(RequestDelegate next, IMemoryCache memoryCache) 
{
    public async Task Invoke(HttpContext context)
    {
        var IpAddress = context.Connection.RemoteIpAddress?.ToString();
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var query = context.Request.QueryString.HasValue? context.Request.QueryString.Value : string.Empty;
        //requestBody 

        var bodyString = string.Empty;

        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            bodyString = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var combinedKey = $"{IpAddress}-{method}-{path}-{query}-{bodyString}";
        var cacheKey = ComputeMD5Hash(combinedKey);

        if (memoryCache.TryGetValue(cacheKey, out CachedResponse? cachedResponse))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = cachedResponse!.StatusCode;
            await context.Response.WriteAsync(cachedResponse.Content);
            return;
        }

        var originalResponseBody = context.Response.Body;
        using var newResponseBody = new MemoryStream();
        context.Response.Body = newResponseBody;

        await next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = new StreamReader(context.Response.Body).ReadToEnd();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var cacheEntry = new CachedResponse
        {
            StatusCode = context.Response.StatusCode,
            Content = responseText
        };

        memoryCache.Set(cacheKey, cacheEntry, TimeSpan.FromSeconds(5));

        await newResponseBody.CopyToAsync(originalResponseBody);
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

    public class CachedResponse
    {
        public int StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
