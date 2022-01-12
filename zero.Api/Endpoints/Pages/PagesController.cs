using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Pages;

public class PagesController : ZeroApiTreeEntityStoreController<Page, IPagesStore>
{
  IPageTypeService PageTypeService;
  IRoutes Routes;

  public PagesController(IPagesStore store, IPageTypeService pageTypeService, IRoutes routes) : base(store)
  {
    PageTypeService = pageTypeService;
    Routes = routes;
  }


  [HttpGet("empty")]
  [ZeroAuthorize(PagePermissions.Create)]
  public virtual async Task<ActionResult<object>> Empty(string flavor, string parentId = null)
  {
    Page page = await Store.Empty(flavor, parentId);

    if (page == null)
    {
      return NotFound();
    }

    if (!await Store.IsAllowedAsChild(page, parentId))
    {
      return BadRequest(Result.Fail("@errors.childnotallowed"));
    }

    PageEdit model = new()
    {
      Id = page.Id,
      Page = page,
      PageType = PageTypeService.GetByAlias(page.Flavor)
    };

    return model;
  }


  [HttpGet("{id}/flavors")]
  [ZeroAuthorize(PagePermissions.Read)]
  public async Task<ActionResult<IEnumerable<FlavorConfig>>> GetAllowedFlavors(string id) => Ok(await PageTypeService.GetAllowedTypes(NormalizeParentId(id)));


  [HttpGet("{id}")]
  [ZeroAuthorize(PagePermissions.Read)]
  public virtual async Task<ActionResult<PageEdit>> Get(string id, string changeVector = null)
  {
    Page page = await Store.Load(id, changeVector);

    if (page == null)
    {
      return NotFound();
    }

    string url = await Routes.GetUrl(page);

    PageEdit model = new()
    {
      Id = page.Id,
      Page = page,
      PageType = PageTypeService.GetByAlias(page.Flavor),
      Urls = url.HasValue() ? new() { url } : new() { }
    };

    return model;
  }


  [HttpGet("{id}/children")]
  [ZeroAuthorize(PagePermissions.Read)]
  public virtual async Task<ActionResult<Paged>> GetChildren(string id, [FromQuery] ListQuery<Page> query)
  {
    query.SearchFor(x => x.Name);
    query.OrderQuery = q => q.OrderByDescending(x => x.Sort).ThenByDescending(x => x.CreatedDate);
    Paged<Page> result = await Store.LoadChildren<zero_Api_Pages_Listing>(id, query.Page, query.PageSize, q => q.Filter(query));

    return result;
  }

  [HttpGet("")]
  [ZeroAuthorize(PagePermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Page> query)
  {
    query.SearchFor(x => x.Name);
    query.OrderQuery = q => q.OrderByDescending(x => x.Sort).ThenByDescending(x => x.CreatedDate);
    return GetModelsByIndex<zero_Api_Pages_Listing>(query);
  }

  [HttpPost("")]
  [ZeroAuthorize(PagePermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(Page saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(PagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, Page updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(PagePermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModelWithDescendants(id);


  [HttpPut("{id}/move/{destinationId}")]
  [ZeroAuthorize(PagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Move(string id, string destinationId) => MoveModel(id, NormalizeParentId(destinationId));


  [HttpPut("{id}/copy/{destinationId}")]
  [ZeroAuthorize(PagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Move(string id, string destinationId, [FromQuery] bool includeDescendents = false)
  {
    destinationId = NormalizeParentId(destinationId);

    return includeDescendents ?
      CopyModelWithDescendants(id, destinationId)
      : CopyModel(id, destinationId);
  }


  [HttpPut("sort")]
  [ZeroAuthorize(PagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Sort([FromBody] string[] ids) => SortModels(ids);
}