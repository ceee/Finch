using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeController
  {
    private ICountriesApi Api { get; set; }

    public CountriesController(IZeroConfiguration config, ICountriesApi api, IMapper mapper, IToken token) : base(config, mapper, token)
    {
      Api = api;
    }


    /// <summary>
    /// Get country by id
    /// </summary>
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Country, CountryEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all countries
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Country> query)
    {
      return await As<Country, CountryListModel>(await Api.GetByQuery("en-US", query));
    }


    /// <summary>
    /// Save country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] CountryEditModel model)
    {
      Country country = await Mapper.Map(model, await Api.GetById(model.Id));
      return Json(await Api.Save(country));
    }


    /// <summary>
    /// Deletes a country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return Json(await Api.Delete(id));
    }
  }
}
