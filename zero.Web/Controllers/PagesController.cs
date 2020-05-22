using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class PagesController : BackofficeController
  {
    IPagesApi Api;

    public PagesController(IPagesApi api)
    {
      Api = api;
    }


    /// <summary>
    /// Get all page types which are allowed below a selected parent page
    /// </summary>
    public async Task<ActionResult> GetAllowedPageTypes([FromQuery] string parent = null)
    {
      return Json(await Api.GetAllowedPageTypes(parent));
    }
  }
}
