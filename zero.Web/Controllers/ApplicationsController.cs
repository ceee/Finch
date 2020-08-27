using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Read)]
  public class ApplicationsController : BackofficeController
  {
    IApplicationsApi Api;
    IApplication Blueprint;

    public ApplicationsController(IApplicationsApi api, IApplication blueprint)
    {
      Api = api;
      Blueprint = blueprint;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty() => Edit(Blueprint.Clone());


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<IApplication> query = null)
    {
      if (query == null)
      {
        return Json(await Api.GetAll());
      }

      return Json(await Api.GetByQuery(query));
    }


    /// <summary>
    /// Get all available features to select
    /// </summary>
    public IActionResult GetAllFeatures() => Json(Options.Features.GetAllItems());


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] IApplication model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
