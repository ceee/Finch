using Microsoft.AspNetCore.Mvc;
using System;
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


    public IActionResult GetModuleTypes() => Json(Api.GetModuleTypes());
 
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
