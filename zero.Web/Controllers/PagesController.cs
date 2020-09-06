using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PagesController : BackofficeController
  {
    IPagesApi Api;
    IRevisionsApi RevisionsApi;

    public PagesController(IPagesApi api, IRevisionsApi revisionsApi)
    {
      Api = api;
      RevisionsApi = revisionsApi;
    }


    public async Task<ActionResult> GetAllowedPageTypes([FromQuery] string parent = null) => Json(await Api.GetAllowedPageTypes(parent));

 
    public IActionResult GetPageType([FromQuery] string alias) => Json(Api.GetPageType(alias));

    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      IPage entity = await Api.GetById(id);

      if (entity == null)
      {
        return NotFound();
      }

      return Edit<IPage, PageEditModel<IPage>>(new PageEditModel<IPage>()
      {
        Entity = entity,
        //Revisions = await RevisionsApi.GetPaged<IPage>(id),
        PageType = Api.GetPageType(entity.PageTypeAlias)
      });
    }

    public IActionResult GetEmpty(string type, string parent = null)
    {
      PageType pageType = Api.GetPageType(type);
      IPage model = Activator.CreateInstance(pageType.ContentType) as IPage;

      model.PageTypeAlias = type;
      model.ParentId = parent;

      return Edit(model);
    }

    public async Task<IActionResult> GetRevisions([FromQuery] string id, [FromQuery] int page = 1) => Json(await RevisionsApi.GetPaged<IPage>(id, page));

    public async Task<IActionResult> Save([FromBody] IPage model) => Json(await Api.Save(model));

    [HttpPost]
    public async Task<IActionResult> SaveSorting([FromBody] string[] ids) => Json(await Api.SaveSorting(ids));

    [HttpPost]
    public async Task<IActionResult> Move([FromBody] ActionCopyModel model) => Json(await Api.Move(model.Id, model.DestinationId));

    [HttpPost]
    public async Task<IActionResult> Copy([FromBody] ActionCopyModel model) => Json(await Api.Copy(model.Id, model.DestinationId, model.IncludeDescendants));

    [HttpPost]
    public async Task<IActionResult> Restore([FromBody] ActionCopyModel model) => Json(await Api.Restore(model.Id, model.IncludeDescendants));

    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id, true));
  }
}
