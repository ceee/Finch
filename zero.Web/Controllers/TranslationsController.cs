using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Read)]
  public class TranslationsController : BackofficeController
  {
    private ITranslationsApi Api { get; set; }

    private ZeroOptions Options { get; set; }


    public TranslationsController(IZeroConfiguration config, ITranslationsApi api, IMapper mapper, IToken token, IOptionsMonitor<ZeroOptions> options) : base(config, mapper, token)
    {
      Api = api;
      Options = options.CurrentValue;
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
      return await As<Translation, TranslationListModel>(await Api.GetByQuery("en-US", query));
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
