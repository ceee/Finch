using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController : BackofficeController
  {
    ILanguagesApi Api;


    public LanguagesController(ILanguagesApi api)
    {
      Api = api;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty()
    {
      return Json(new LanguageEditModel());
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Language, LanguageEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Language> query)
    {
      return await As<Language, LanguageListModel>(await Api.GetByQuery(query));
    }


    /// <summary>
    /// Returns all cultures available for creating languages.
    /// </summary>
    public IActionResult GetAllCultures()
    {
      return Json(Api.GetAllCultures());
    }


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] LanguageEditModel model)
    {
      Language entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<Language, LanguageEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Language, LanguageEditModel>(await Api.Delete(id));
    }
  }
}
