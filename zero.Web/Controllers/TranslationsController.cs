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


    public EditModel<ITranslation> GetEmpty([FromServices] ITranslation blueprint) => Edit(blueprint);


    public async Task<EditModel<ITranslation>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<ListResult<ITranslation>> GetAll([FromQuery] ListQuery<ITranslation> query) => await Api.GetByQuery(query);


    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Update)]
    public async Task<EntityResult<ITranslation>> Save([FromBody] ITranslation model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Update)]
    public async Task<EntityResult<ITranslation>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
