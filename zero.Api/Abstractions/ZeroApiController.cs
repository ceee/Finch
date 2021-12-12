using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Abstractions;

[ApiController]
[ZeroAuthorize]
[DisableBrowserCacheFilter]
[ApiMetadataFilter]
[ApiResponseFilter]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroApiController : ControllerBase
{
  IZeroMapper _mapper;
  protected IZeroMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IZeroMapper>());


  protected ApiRequestHints Hints { get; set; } = new();
}
