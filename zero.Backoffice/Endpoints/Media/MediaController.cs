//using Microsoft.AspNetCore.Mvc;
//using Raven.Client.Documents;
//using zero.Api.Models;

//namespace zero.Backoffice.Endpoints.Media;

//public class MediaController : ZeroBackofficeController
//{
//  readonly IPagesStore Store;
//  readonly IMediaTreeService PageTreeService;
//  readonly IPageTypeService PageTypeService;
//  readonly IRoutes Routes;


//  public PagesController(IPagesStore store, IMediaTreeService pageTreeService, IPageTypeService pageTypeService, IRoutes routes)
//  {
//    Store = store;
//    PageTreeService = pageTreeService;
//    PageTypeService = pageTypeService;
//    Routes = routes;
//  }


//  [HttpGet("{parentId}/children")]
//  public async Task<ActionResult<List<TreeItem>>> GetChildren(string parentId = null, string activeId = null, string search = null)
//  {
//    return await PageTreeService.GetChildren(parentId, activeId, search);
//  }


//  [HttpGet("{parentId}/dependencies")]
//  //[ZeroAuthorize(MediaPermissions.Update)]
//  public virtual async Task<ActionResult<dynamic>> GetDependencies(string parentId)
//  {
//    int descendantCount = await Store.Session.Query<zero_Pages_ByHierarchy.Result, zero_Pages_ByHierarchy>()
//      .ProjectInto<zero_Pages_ByHierarchy.Result>()
//      .CountAsync(x => x.PathIds.Contains(parentId));

//    return new
//    {
//      pages = descendantCount + 1
//    };
//  }


//  [HttpGet("previews")]
//  public async Task<ActionResult<List<LinkPreview>>> GetPreviews([FromQuery] List<string> ids)
//  {
//    IEnumerable<FlavorConfig> pageTypes = PageTypeService.GetAll();
//    Dictionary<string, Page> pages = await Store.Load(ids.ToArray());
//    Dictionary<Page, Route> routes = await Routes.GetRoutes(pages.Where(x => x.Value != null).Select(x => x.Value).ToArray());

//    List<LinkPreview> previews = new();

//    foreach ((string id, Page page) in pages)
//    {
//      if (page != null)
//      {
//        routes.TryGetValue(page, out Route route);
//        FlavorConfig pageType = PageTypeService.GetByAlias(page.Flavor);

//        previews.Add(new()
//        {
//          Name = page.Name,
//          Text = route?.Url.Or("@page.picker.urlnotfound"),
//          Id = page.Id,
//          Icon = pageType?.Icon.Or("fth-folder")
//        });
//      }
//      else
//      {
//        previews.Add(new()
//        {
//          HasError = true,
//          Icon = "fth-alert-circle color-red",
//          Id = "tmp_" + IdGenerator.Create(),
//          Name = "@errors.preview.notfound",
//          Text = "@errors.preview.notfound_text"
//        });
//      }
//    }

//    return previews;
//  }
//}