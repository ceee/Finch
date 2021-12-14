using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace zero.Api.Filters;

public class ApiUnhandledExceptionMiddleware : IMiddleware
{
  private readonly ILogger<ApiUnhandledExceptionMiddleware> _logger;

  public ApiUnhandledExceptionMiddleware(ILogger<ApiUnhandledExceptionMiddleware> logger)
  {
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (Exception e)
    {
      _logger.LogError(e, "API Exception thrown at '{path}'", context.Request.GetEncodedPathAndQuery());
      throw;
    }
  }
}