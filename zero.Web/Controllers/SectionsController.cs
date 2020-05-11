using Microsoft.AspNetCore.Mvc;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class SectionsController : BackofficeController
  {
    ISectionsApi Api;

    public SectionsController(ISectionsApi api)
    {
      Api = api;
    }


    public IActionResult GetAll()
    {
      return Json(Api.GetAll());
    }
  }
}
