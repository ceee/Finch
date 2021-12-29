using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Abstractions;

[ApiController]
[ZeroAuthorize]
[ApiMetadataFilter]
[ApiResponseFilter]
[TypeFilter(typeof(ApiExceptionFilter))]
//[MiddlewareFilter(typeof(ApiUnhandledExceptionMiddlewareFilter))]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroApiController : ControllerBase
{
  IZeroMapper _mapper;
  protected IZeroMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IZeroMapper>());


  public ApiRequestHints Hints { get; protected set; } = new();
}
