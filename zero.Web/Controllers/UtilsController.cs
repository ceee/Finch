using Microsoft.AspNetCore.Mvc;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class UtilsController : BackofficeController
  {
    /// <summary>
    /// Generate alias from name
    /// </summary>  
    public IActionResult GenerateAlias([FromQuery] string name) => Json(Safenames.Alias(name));
  }
}
