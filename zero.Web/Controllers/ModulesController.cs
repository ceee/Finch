using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Utils;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class ModulesController : BackofficeController
  {
    IModulesApi Api;

    public ModulesController(IModulesApi api)
    {
      Api = api;
    }


    public async Task<IList<ModuleType>> GetModuleTypes([FromQuery] string[] tags = default, [FromQuery] string pageId = default) => await Api.GetModuleTypes(tags, pageId);
 

    public ModuleType GetModuleType([FromQuery] string alias) => Api.GetModuleType(alias);


    public EditModel<IModule> GetEmpty(string alias)
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
