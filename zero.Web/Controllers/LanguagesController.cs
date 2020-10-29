using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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


    public EditModel<ILanguage> GetEmpty([FromServices] ILanguage blueprint) => Edit(blueprint);


    public async Task<EditModel<ILanguage>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<ListResult<ILanguage>> GetAll([FromQuery] ListQuery<ILanguage> query) => await Api.GetByQuery(query);


    public IList<Culture> GetAllCultures() => Api.GetAllCultures();


    public IList<Culture> GetSupportedCultures() => Api.GetAllCultures(Options.SupportedLanguages);


    public async Task<IEnumerable<SelectModel>> GetForPicker() => (await Api.GetAll()).Select(x => new SelectModel()
    {
      Id = x.Id,
      Name = x.Name,
      IsActive = x.IsActive
    });


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      return Previews(await Api.GetByIds(ids.ToArray()), item => new PreviewModel()
      {
        Id = item.Id,
        Icon = "fth-globe",
        Name = item.Name
      });
    }


    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Update)]
    public async Task<EntityResult<ILanguage>> Save([FromBody] ILanguage model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Update)]
    public async Task<EntityResult<ILanguage>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
