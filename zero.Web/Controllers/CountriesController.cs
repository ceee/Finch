using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Backoffice;
using zero.Core.Entities;
using zero.Core.Extensions;
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

    public override void OnScopeChanged(string scope)
    {
      Service.ApplyScope(scope);
    }


    public async Task<EditModel<ICountry>> GetById([FromQuery] string id) => Edit(await Service.GetById(id)); 


    public EditModel<ICountry> GetEmpty([FromServices] ICountry blueprint) => Edit(blueprint);


    public async Task<ListResult<ICountry>> GetByQuery([FromQuery] ListQuery<ICountry> query)
    {
      query.SearchSelector = country => country.Name;
      return await Service.Query.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name).ToQueriedListAsync(query);
    }


    public async Task<IEnumerable<SelectModel>> GetForPicker() => await SelectList(Service.Stream());


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      return Previews(await Service.GetByIds(ids.ToArray()), (item, model) => model.Icon = "flag flag-" + item.Code.ToLowerInvariant());
    }


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Save([FromBody] ICountry model) => await Service.Save(model);


    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Update)]
    public async Task<EntityResult<ICountry>> Delete([FromQuery] string id) => await Service.DeleteById(id);
  }
}
