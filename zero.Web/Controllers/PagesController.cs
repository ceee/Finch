using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PagesController<T> : BackofficeController where T : IPage, new()
  {
    IPagesApi<T> Api;
    IRevisionsApi RevisionsApi;

    public PagesController(IPagesApi<T> api, IRevisionsApi revisionsApi)
    {
      Api = api;
      RevisionsApi = revisionsApi;
    }


    public async Task<ActionResult> GetAllowedPageTypes([FromQuery] string parent = null) => Json(await Api.GetAllowedPageTypes(parent));

 
    public IActionResult GetPageType([FromQuery] string alias) => Json(Api.GetPageType(alias));

    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      T entity = await Api.GetById(id);

      return Edit<T, PageEditModel<T>>(new PageEditModel<T>()
      {
        Entity = entity,
        Revisions = await RevisionsApi.GetPaged<T>(id),
        PageType = Api.GetPageType(entity.PageTypeAlias)
      });
    }

    public IActionResult GetEmpty(string type, string parent = null) => Edit(new T()
    {
      PageTypeAlias = type,
      ParentId = parent
    });

    public async Task<IActionResult> GetRevisions([FromQuery] string id, [FromQuery] int page = 1) => Json(await RevisionsApi.GetPaged<T>(id, page));

    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));

    [HttpPost]
    public async Task<IActionResult> SaveSorting([FromBody] string[] ids) => Json(await Api.SaveSorting(ids));

    [HttpPost]
    public async Task<IActionResult> Move([FromBody] CopyModel model) => Json(await Api.Move(model.Id, model.DestinationId));

    [HttpPost]
    public async Task<IActionResult> Copy([FromBody] CopyModel model) => Json(await Api.Copy(model.Id, model.DestinationId, model.IncludeDescendants));


    public async Task<IActionResult> Delete([FromQuery] string id, [FromQuery] bool moveToRecycleBin = false) => Json(await Api.Delete(id, moveToRecycleBin));


    public class CopyModel
    {
      public string Id { get; set; }

      public string DestinationId { get; set; }

      public bool IncludeDescendants { get; set; }
    }
  }
}
