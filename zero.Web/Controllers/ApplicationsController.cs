using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Read)]
  public class ApplicationsController : BackofficeController
  {
    IApplicationsApi Api;

    public ApplicationsController(IApplicationsApi api)
    {
      Api = api;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty()
    {
      return Json(new ApplicationEditModel());
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Application, ApplicationEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Application> query = null)
    {
      if (query == null)
      {
        return await As<Application, ApplicationListModel>(await Api.GetAll());
      }

      return await As<Application, ApplicationListModel>(await Api.GetByQuery(query));
    }


    /// <summary>
    /// Get all available features to select
    /// </summary>
    public IActionResult GetAllFeatures()
    {
      return Json(Options.Features.GetAllItems());
    }


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] ApplicationEditModel model)
    {
      Application entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<Application, ApplicationEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Application, ApplicationEditModel>(await Api.Delete(id));
    }


    /// <summary>
    /// Save translation
    /// </summary>
    //public async Task<IActionResult> Switch([FromQuery] string id)
    //{
    //  return await As<Application, ApplicationEditModel>(await Api.Save(entity));
    //}
  }
}
