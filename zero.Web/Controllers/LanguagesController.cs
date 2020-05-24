using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController<T> : BackofficeController where T : ILanguage, new()
  {
    ILanguagesApi<T> Api;


    public LanguagesController(ILanguagesApi<T> api)
    {
      Api = api;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty() => JsonEdit(new T());


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id) => JsonEdit(await Api.GetById(id));


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<T> query) => Json(await Api.GetByQuery(query));


    /// <summary>
    /// Returns all cultures available for creating languages.
    /// </summary>
    public IActionResult GetAllCultures() => Json(Api.GetAllCultures());


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
