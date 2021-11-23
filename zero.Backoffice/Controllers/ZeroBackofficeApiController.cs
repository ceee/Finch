using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Controllers;

[ApiController]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroBackofficeApiController : ZeroBackofficeController
{
  
}
