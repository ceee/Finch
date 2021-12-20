using Microsoft.AspNetCore.Mvc;
using zero.Api.Models;

namespace zero.Backoffice.Endpoints.Pages;

public class PagesController : ZeroBackofficeController
{
  readonly IPageTreeService PageTreeService;


  public PagesController(IPageTreeService pageTreeService)
  {
    PageTreeService = pageTreeService;
  }


  [HttpGet("{parentId}/children")]
  public async Task<ActionResult<List<TreeItem>>> GetChildren(string parentId = null, string activeId = null, string search = null)
  {
    return await PageTreeService.GetChildren(parentId, activeId, search);
  }
}