using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.PageModules;

public class PageModulesController : ZeroApiController
{
  readonly IPageModuleTypeService ModuleTypeService;

  public PageModulesController(IPageModuleTypeService moduleTypeService)
  {
    ModuleTypeService = moduleTypeService;
  }


  [HttpGet("")]
  [ZeroAuthorize(PageModulePermissions.Read)]
  public async Task<ActionResult<IList<PageModuleType>>> GetModuleTypes([FromQuery] string[] tags = default, [FromQuery] string pageId = default) => Ok(await ModuleTypeService.GetModuleTypes(tags, pageId));


  [HttpGet("{alias}")]
  [ZeroAuthorize(PageModulePermissions.Read)]
  public ActionResult<PageModuleType> GetModuleType(string alias) => Ok(ModuleTypeService.GetModuleType(alias));


  [HttpGet("{alias}/empty")]
  [ZeroAuthorize(PageModulePermissions.Read)]
  public ActionResult<PageModule> GetEmpty(string alias)
  {
    PageModule module = ModuleTypeService.GetEmpty(alias);

    if (module == null)
    {
      return NotFound();
    }

    return module;
  }
}