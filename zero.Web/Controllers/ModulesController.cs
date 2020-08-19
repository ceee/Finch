using Microsoft.AspNetCore.Mvc;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class ModulesController : BackofficeController
  {
    IModulesApi Api;

    public ModulesController(IModulesApi api)
    {
      Api = api;
    }


    public IActionResult GetModuleTypes() => Json(Api.GetModuleTypes());
 
    public IActionResult GetModuleType([FromQuery] string alias) => Json(Api.GetModuleType(alias));
  }
}
