using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class PageTreeController : BackofficeController
  {
    IPageTreeApi Api;

    public PageTreeController(IPageTreeApi api)
    {
      Api = api;
    }


    public async Task<IActionResult> GetChildren(string parent = null)
    {
      return AsJson(await Api.GetChildren(parent));
    }
  }
}
