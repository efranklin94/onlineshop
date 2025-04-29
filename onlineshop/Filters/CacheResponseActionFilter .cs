using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using onlineshop.Features;

namespace onlineshop.Filters
{
    public class CacheResponseActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Console.WriteLine("CacheResponseActionFilter invoked");

            var executedContext = await next();
            if (executedContext.Result is ObjectResult objectResult)
            {
                var value = objectResult.Value;
                context.HttpContext.Items["IdempotencyResponse"] = value;
                Console.WriteLine("CacheResponseActionFilter invoked2");
            }


        }
    }
}
