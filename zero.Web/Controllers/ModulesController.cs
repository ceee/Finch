using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Utils;

namespace zero.Web.Controllers
{
  public class ModulesController : BackofficeController
  {
    IModulesApi Api;

    public ModulesController(IModulesApi api)
    {
      Api = api;
    }


    public IActionResult GetModuleTypes([FromQuery] string[] tags = default) => Json(Api.GetModuleTypes(tags));
 
    public IActionResult GetModuleType([FromQuery] string alias) => Json(Api.GetModuleType(alias));

    public IActionResult GetEmpty(string alias)
    {
      ModuleType moduleType = Api.GetModuleType(alias);
      IModule module = Activator.CreateInstance(moduleType.ContentType) as IModule;

      module.ModuleTypeAlias = moduleType.Alias;
      module.Id = IdGenerator.Create(8);
      module.IsActive = true;

      return Edit(module);
    }
  }
}
