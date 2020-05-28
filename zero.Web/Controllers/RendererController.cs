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
      RendererConfig config = null;
      AbstractGenericRenderer renderer = Options.Renderers.GetAllItems().FirstOrDefault(x => x.Alias == alias);

      config = renderer?.Build();

      JsonSerializerSettings settings = JsonConvert.DefaultSettings();
      settings.TypeNameHandling = TypeNameHandling.Objects;

      return Json(config, settings);
    }
  }
}
