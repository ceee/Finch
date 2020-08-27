using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeController
  {
    ICountriesApi Api;
    ICountry Blueprint;

    public CountriesController(ICountriesApi api, ICountry blueprint)
    {
      Api = api;
      Blueprint = blueprint;
    }


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public IActionResult GetEmpty() => Edit(Blueprint.Clone());


    public async Task<IActionResult> GetAll([FromQuery] ListQuery<ICountry> query) => Json(await Api.GetByQuery("languages.1-A", query)); // TODO correct language


    public async Task<IActionResult> GetForPicker() => Json((await Api.GetAll("languages.1-A")).Select(x => new SelectModel()
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
        Icon = "flag flag-" + item.Code.ToLowerInvariant(),
        Name = item.Name
      });
    }


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] ICountry model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
