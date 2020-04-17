using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeController
  {
    private ICountriesApi Api { get; set; }

    public CountriesController(IZeroConfiguration config, ICountriesApi api) : base(config)
    {
      Api = api;
    }


    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Country> query)
    {
      return Json(await Api.GetByQuery("en-US", query));
    }
  }
}
