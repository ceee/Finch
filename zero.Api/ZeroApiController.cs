using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using zero.Api.Filters;

namespace zero.Api.Controllers;

[ApiController]
[ZeroAuthorize]
[DisableBrowserCache]
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
