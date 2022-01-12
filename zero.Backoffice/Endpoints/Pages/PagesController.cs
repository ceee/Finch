using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using zero.Api.Models;

namespace zero.Backoffice.Endpoints.Pages;

public class PagesController : ZeroBackofficeController
{
  readonly IPagesStore Store;
  readonly IPageTreeService PageTreeService;


  public PagesController(IPagesStore store, IPageTreeService pageTreeService)
  {
    Store = store;
    PageTreeService = pageTreeService;
  }


  [HttpGet("{parentId}/children")]
  public async Task<ActionResult<List<TreeItem>>> GetChildren(string parentId = null, string activeId = null, string search = null)
  {
    return await PageTreeService.GetChildren(parentId, activeId, search);
  }


  [HttpGet("{parentId}/dependencies")]
  //[ZeroAuthorize(MediaPermissions.Update)]
  public virtual async Task<ActionResult<dynamic>> GetDependencies(string parentId)
  {
    int descendantCount = await Store.Session.Query<zero_Pages_ByHierarchy.Result, zero_Pages_ByHierarchy>()
      .ProjectInto<zero_Pages_ByHierarchy.Result>()
      .CountAsync(x => x.PathIds.Contains(parentId));

    return new
    {
      pages = descendantCount + 1
    };
  }
}