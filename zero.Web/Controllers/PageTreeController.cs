using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class PageTreeController : BackofficeController
  {
    IPageTreeApi Api;
    IWebHostEnvironment Env;

    public PageTreeController(IPageTreeApi api, IWebHostEnvironment env)
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
