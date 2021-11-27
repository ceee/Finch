using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize]
[DisableBrowserCache]
public abstract class ZeroBackofficeController : ControllerBase
{
  
}
