using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class PageTreeController : BackofficeController
  {
    private IPageTreeApi Api { get; set; }

    private IWebHostEnvironment Env { get; set; }

    public PageTreeController(IZeroConfiguration config, IPageTreeApi api, IWebHostEnvironment env) : base(config)
    {
      Api = api;
      Env = env;
    }


    public async Task<IActionResult> GetChildren(string parent = null)
    {
      return Json(await Api.GetChildren(Env.ContentRootPath, parent));
    }
  }
}
