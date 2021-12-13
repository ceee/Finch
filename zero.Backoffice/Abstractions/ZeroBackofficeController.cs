using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Abstractions;

[ApiController]
[ZeroAuthorize]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroBackofficeController : ControllerBase
{
  IZeroMapper _mapper;
  protected IZeroMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IZeroMapper>());
}
