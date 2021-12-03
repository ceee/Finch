using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using zero.Api.Controllers;

namespace zero.Backoffice.Controllers;

[ApiController]
[ZeroAuthorize]
[DisableBrowserCache]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroBackofficeController : ControllerBase
{
  IZeroMapper _mapper;
  protected IZeroMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IZeroMapper>());
}
