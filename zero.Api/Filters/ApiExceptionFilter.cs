using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace zero.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
  readonly IHostEnvironment _hostEnvironment;
  readonly ILogger<ApiExceptionFilter> _logger;

  public ApiExceptionFilter(IHostEnvironment hostEnvironment, ILogger<ApiExceptionFilter> logger)
  {
    _hostEnvironment = hostEnvironment;
    _logger = logger;
  }

  public void OnException(ExceptionContext context)
  {
    if (!_hostEnvironment.IsDevelopment())
    {
      return;
    }

    _logger.LogError(context.Exception, "API Exception thrown at '{path}'", context.HttpContext.Request.GetEncodedPathAndQuery());

    JsonResult result = new(new ErrorApiResponse()
    {
      Success = false,
      Status = StatusCodes.Status500InternalServerError,
      Metadata = GetMetadata(context),
      Errors = new()
      {
        new ErrorApiResponseError()
        {
          Code = ApiErrorCodes.Server.Exception,
          Category = ApiErrorCodes.Categories.Server,
          Message = context.Exception.Message
        }
      }
      //Content = context.Exception.ToString()
    });

    result.StatusCode = StatusCodes.Status500InternalServerError;

    context.Result = result;
  }


  ApiResponseMetadata GetMetadata(ExceptionContext context)
  {
    DateTimeOffset started = (DateTimeOffset)context.HttpContext.Items["zero.action.started"];
    TimeSpan duration = DateTimeOffset.Now - started;

    return new()
    {
      Duration = duration,
      RequestDate = started
    };
  }
}