using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController<T> : BackofficeController where T : ICountry, new()
  {
    ICountriesApi<T> Api;

    public CountriesController(ICountriesApi<T> api)
    {
      Api = api;
    }


    /// <summary>
    /// Get country by id
    /// </summary>
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    /// <summary>
    /// Get new country
    /// </summary>  
    public IActionResult GetEmpty() => Edit(new T());


    /// <summary>
    /// Get all countries
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<T> query) => Json(await Api.GetByQuery("languages.1-A", query)); // TODO correct language


    /// <summary>
    /// Save country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    /// <summary>
    /// Deletes a country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
