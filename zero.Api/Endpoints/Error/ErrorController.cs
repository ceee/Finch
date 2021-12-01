using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Error;

public class ErrorController : ZeroApiController
{
  [HttpGet("")]
  public virtual ActionResult<ApiResponse> Index()
  {
    IExceptionHandlerFeature exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

    return new ErrorApiResponse()
    {
      Success = false,
      Status = HttpContext.Response.StatusCode,
      Error = new()
      {
        ApiPath = exception.Path,
        Message = exception.Error.Message
      }
    };
  }
}