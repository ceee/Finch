using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Read)]
  public class TranslationsController : BackofficeController
  {
    ITranslationsApi Api;
    ITranslation Blueprint;

    public TranslationsController(ITranslationsApi api, ITranslation blueprint)
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
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<ITranslation> query) => Json(await Api.GetByQuery(query));


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] ITranslation model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
