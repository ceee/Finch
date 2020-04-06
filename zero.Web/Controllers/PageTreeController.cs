using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  [AllowAnonymous]
  public class PageTreeController : BackofficeController
  {
    private IPageTreeApi Api { get; set; }

    private IWebHostEnvironment Env { get; set; }

    public PageTreeController(IZeroConfiguration config, IPageTreeApi api, IWebHostEnvironment env) : base(config)
    {
      Api = api;
      Env = env;
    }


    public IActionResult GetChildren(string parent = null)
    {
      return Json(Api.GetChildren(Env.ContentRootPath, parent));
    }
  }
}
