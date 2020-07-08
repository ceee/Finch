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


    public async Task<ActionResult> GetAllowedPageTypes([FromQuery] string parent = null) => Json(await Api.GetAllowedPageTypes(parent));

 
    public IActionResult GetPageType([FromQuery] string alias) => Json(Api.GetPageType(alias));


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
