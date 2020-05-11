using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Read)]
  public class TranslationsController : BackofficeController
  {
    ITranslationsApi Api;

    public TranslationsController(ITranslationsApi api)
    {
      Api = api;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty()
    {
      return Json(new TranslationEditModel());
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Translation, TranslationEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Translation> query)
    {
      return await As<Translation, TranslationListModel>(await Api.GetByQuery(query));
    }


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] TranslationEditModel model)
    {
      Translation entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<Translation, TranslationEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Translation, TranslationEditModel>(await Api.Delete(id));
    }
  }
}
