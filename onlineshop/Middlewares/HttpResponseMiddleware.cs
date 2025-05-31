﻿using Application.Exceptions;
using Resources;

namespace API.Middlewares;

public class HttpResponseMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        await next(context);

        switch (context.Response.StatusCode)
        {
            case StatusCodes.Status401Unauthorized: throw new UnauthorizedException(Messages.Unauthorized);
            case StatusCodes.Status403Forbidden: throw new ForbiddenException(Messages.Forbidden);
        }
    }
}
