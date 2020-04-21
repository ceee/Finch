using Microsoft.AspNetCore.Authorization;
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

    public CountriesController(IZeroConfiguration config, ICountriesApi api, IMapper mapper) : base(config, mapper)
    {
      Api = api;
    }


    /// <summary>
    /// Get country by id
    /// </summary>    
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return Json<Country, CountryEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all countries
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Country> query)
    {
      return Json(await Api.GetByQuery("en-US", query));
    }
  }
}
