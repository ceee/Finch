using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class PagesController<T> : BackofficeController where T : IPage
  {
    IPagesApi<T> Api;

    public PagesController(IPagesApi<T> api)
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


    /// <summary>
    /// Get translation by id
    /// </summary>  
    //public IActionResult GetEmpty() => JsonEdit(new T());


    /// <summary>
    /// Get page by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));
  }
}
