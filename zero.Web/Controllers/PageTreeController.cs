using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class PageTreeController : BackofficeController
  {
    IPageTreeApi Api;

    public PageTreeController(IPageTreeApi api)
    {
      Api = api;
    }


    public async Task<IActionResult> GetChildren(string parent = null, string active = null)
    {
      return Json(await Api.GetChildren(parent, active));
    }
  }
}
