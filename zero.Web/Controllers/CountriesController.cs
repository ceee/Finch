using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Backoffice;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeController
  {
    ICountriesBackofficeService Service;

    public CountriesController(ICountriesBackofficeService service)
    {
      Service = service;
    }


    public async Task<EditModel<ICountry>> GetById([FromQuery] string id) => Edit(await Service.GetById(id));

    public EditModel<ICountry> GetEmpty([FromServices] ICountry blueprint) => Edit(blueprint);

    public async Task<ListResult<ICountry>> GetAll([FromQuery] ListQuery<ICountry> query) => await Service.GetByQuery(query);

    public async Task<IEnumerable<SelectModel>> GetForPicker() => await SelectList(Service.Stream());

    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      return Previews(await Service.GetByIds(ids.ToArray()), item => new PreviewModel()
      {
        Id = item.Id,
        Icon = "flag flag-" + item.Code.ToLowerInvariant(),
        Name = item.Name
      });
    }


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Save([FromBody] ICountry model) => await Service.Save(model);


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Delete([FromQuery] string id) => await Service.DeleteById(id);
  }
}
