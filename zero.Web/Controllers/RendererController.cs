using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Identity;
using zero.Core.Renderer;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  public class RendererController : BackofficeController
  {
    /// <summary>
    /// Get a renderer by alias
    /// </summary>    
    public IActionResult GetByAlias([FromServices] IEnumerable<IRenderer> renderers, [FromQuery] string alias)
    {
      IRenderer renderer = renderers.FirstOrDefault(x => x.Alias == alias);
      RendererConfig config = renderer?.Build();

      return Json(config, false);
    }
  }
}
