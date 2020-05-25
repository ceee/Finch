using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class PageTreeController<T> : BackofficeController where T : IPage
  {
    IPageTreeApi<T> Api;

    public PageTreeController(IPageTreeApi<T> api)
    {
      Api = api;
    }


    public async Task<IActionResult> GetChildren(string parent = null, string active = null)
    {
      return Json(await Api.GetChildren(parent, active));
    }
  }
}
