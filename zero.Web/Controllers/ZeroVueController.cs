using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class ZeroVueController : BackofficeController
  {
    private IZeroVue ZeroVue { get; set; }

    public ZeroVueController(IZeroVue zeroVue)
    {
      ZeroVue = zeroVue;
    }


    [HttpGet]
    public async Task<IActionResult> Config()
    {
      var settings = new JsonSerializerSettings();
      settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

      return Json(await ZeroVue.Config()); //, settings);
    }
  }
}
