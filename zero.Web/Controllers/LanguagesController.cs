using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController : BackofficeController
  {
    ILanguagesApi Api;
    ILanguage Blueprint;

    public LanguagesController(ILanguagesApi api, ILanguage blueprint)
    {
      Api = api;
      Blueprint = blueprint;
    }


    /// <summary>
    /// Get empty language model
    /// </summary>  
    public IActionResult GetEmpty() => Edit(Blueprint.Clone());


    /// <summary>
    /// Get language by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    /// <summary>
    /// Get all languages
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<ILanguage> query) => Json(await Api.GetByQuery(query));


    /// <summary>
    /// Returns all cultures available for creating languages.
    /// </summary>
    public IActionResult GetAllCultures() => Json(Api.GetAllCultures());


    /// <summary>
    /// Returns all available backoffice cultures.
    /// </summary>
    public IActionResult GetSupportedCultures() => Json(Api.GetAllCultures(Options.SupportedLanguages));


    public async Task<IActionResult> GetForPicker() => Json((await Api.GetAll()).Select(x => new SelectModel()
    {
      Id = x.Id,
      Name = x.Name,
      IsActive = x.IsActive
    }));


    public async Task<IActionResult> GetPreviews(List<string> ids)
    {
      return JsonPreviews(await Api.GetByIds(ids.ToArray()), item => new PreviewModel()
      {
        Id = item.Id,
        Icon = "fth-globe",
        Name = item.Name
      });
    }


    /// <summary>
    /// Save language
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] ILanguage model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a language
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
