using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Renderer;
using zero.Web.Filters;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController<T> : BackofficeController where T : ILanguage, new()
  {
    ILanguagesApi<T> Api;
    IRenderer<T> Renderer;


    public LanguagesController(ILanguagesApi<T> api, IRenderer<T> renderer)
    {
      Api = api;
      Renderer = renderer;
    }


    /// <summary>
    /// Get empty language model
    /// </summary>  
    public IActionResult GetEmpty() => Edit(new T(), Renderer);


    /// <summary>
    /// Get language by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id), Renderer);


    /// <summary>
    /// Get language renderer
    /// </summary>
    public IActionResult GetRenderer([FromServices] IRenderer<T> renderer) => Json(renderer.Build());


    /// <summary>
    /// Get all languages
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<T> query) => Json(await Api.GetByQuery(query));


    /// <summary>
    /// Returns all cultures available for creating languages.
    /// </summary>
    public IActionResult GetAllCultures() => Json(Api.GetAllCultures());


    /// <summary>
    /// Save language
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a language
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
