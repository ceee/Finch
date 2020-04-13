using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  [AllowAnonymous]
  public class CountriesController : BackofficeController
  {
    private ICountriesApi Api { get; set; }

    public CountriesController(IZeroConfiguration config, ICountriesApi api) : base(config)
    {
      Api = api;
    }


    public async Task<IActionResult> GetAll()
    {
      return Json(await Api.GetAll("en-US"));
    }
  }
}
