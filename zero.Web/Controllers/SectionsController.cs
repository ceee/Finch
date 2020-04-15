using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class SectionsController : BackofficeController
  {
    private ISectionsApi Api { get; set; }

    public SectionsController(IZeroConfiguration config, ISectionsApi api) : base(config)
    {
      Api = api;
    }


    public IActionResult GetAll()
    {
      return Json(Api.GetAll());
    }
  }
}
