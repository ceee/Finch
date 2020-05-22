//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using zero.Core.Api;
//using zero.Core.Entities;
//using zero.Core.Identity;
//using zero.Core.Options;
//using zero.Web.Models;

//namespace zero.Web.Controllers
//{
//  [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Read)]
//  public class ApplicationsController : BackofficeController
//  {
//    IApplicationsApi<Application> Api;

//    public ApplicationsController(IApplicationsApi<Application> api)
//    {
//      Api = api;
//    }


//    /// <summary>
//    /// Get translation by id
//    /// </summary>  
//    public IActionResult GetEmpty([FromServices] IApplication app)
//    {
//      return JsonEdit(app);
//    }


//    /// <summary>
//    /// Get translation by id
//    /// </summary>  
//    public async Task<IActionResult> GetById([FromQuery] string id)
//    {
//      return JsonEdit(await Api.GetById(id));
//    }


//    /// <summary>
//    /// Get all translations
//    /// </summary>    
//    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Application> query = null)
//    {
//      if (query == null)
//      {
//        return Json(await Api.GetAll());
//      }

//      return Json(await Api.GetByQuery(query));
//    }


//    /// <summary>
//    /// Get all available features to select
//    /// </summary>
//    public IActionResult GetAllFeatures()
//    {
//      return Json(Options.Features.GetAllItems());
//    }


//    /// <summary>
//    /// Save translation
//    /// </summary>
//    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
//    public async Task<IActionResult> Save([FromBody] EditModel<IApplication> model)
//    {
//      return Ok();
//      //Application entity = await Mapper.Map(model, await Api.GetById(null));
//      //return await As<IApplication, ApplicationEditModel>(await Api.Save(entity));
//    }


//    /// <summary>
//    /// Deletes a translation
//    /// </summary>
//    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
//    public async Task<IActionResult> Delete([FromQuery] string id)
//    {
//      return JsonEdit(await Api.Delete(id));
//    }


//    /// <summary>
//    /// Save translation
//    /// </summary>
//    //public async Task<IActionResult> Switch([FromQuery] string id)
//    //{
//    //  return await As<Application, ApplicationEditModel>(await Api.Save(entity));
//    //}
//  }
//}
