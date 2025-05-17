using onlineshop.Exceptions;
using onlineshop.Features;
using System.Text.Json;
namespace onlineshop.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            await SetContext(context, ex.Message, ex.Errors, StatusCodes.Status400BadRequest);
        }
        catch (NotFoundException ex)
        {
            await SetContext(context, ex.Message, [], StatusCodes.Status404NotFound);
        }
        catch (TooManyRequestException ex)
        {
            await SetContext(context, ex.Message, [], StatusCodes.Status429TooManyRequests);
        }
        catch (Exception ex)
        {
            await SetContext(context, ex.Message, [], StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task SetContext(HttpContext context, string message, Dictionary<string, string[]> errors, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var result = BaseResult.Fail(message, errors);
        await context.Response.WriteAsync(JsonSerializer.Serialize(message));
    }
}
