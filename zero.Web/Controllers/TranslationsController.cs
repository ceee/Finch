using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Read)]
  public class TranslationsController<T> : BackofficeController where T : ITranslation, new()
  {
    ITranslationsApi<T> Api;

    public TranslationsController(ITranslationsApi<T> api)
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
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
