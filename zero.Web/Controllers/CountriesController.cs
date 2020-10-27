using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeController
  {
    ICountriesApi Api;

    public CountriesController(ICountriesApi api)
    {
      Api = api;
    }


    public async Task<EditModel<ICountry>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public EditModel<ICountry> GetEmpty([FromServices] ICountry blueprint) => Edit(blueprint);


    public async Task<ListResult<ICountry>> GetAll([FromQuery] ListQuery<ICountry> query) => await Api.GetByQuery("languages.1-A", query); // TODO correct language


    public async Task<IEnumerable<SelectModel>> GetForPicker() => (await Api.GetAll("languages.1-A")).Select(x => new SelectModel()
    {
      Id = x.Id,
      Name = x.Name,
      IsActive = x.IsActive
    });


    public async Task<IList<PreviewModel>> GetPreviews(List<string> ids)
    {
      return Previews(await Api.GetByIds(ids.ToArray()), item => new PreviewModel()
      {
        Id = item.Id,
        Icon = "flag flag-" + item.Code.ToLowerInvariant(),
        Name = item.Name
      });
    }


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Save([FromBody] ICountry model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
