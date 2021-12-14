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


  [HttpGet("")]
  [ZeroAuthorize(PagePermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Page> query)
  {
    query.SearchFor(entity => entity.Name);
    return GetModels<PageBasic>(query);
  }


  //[HttpPost("")]
  //[ZeroAuthorize(PagePermissions.Create)]
  //public virtual Task<ActionResult<Result>> Create(MailSave saveModel) => CreateModel<MailSave, MailEdit>(saveModel);


  //[HttpPut("{id}")]
  //[ZeroAuthorize(PagePermissions.Update)]
  //public virtual Task<ActionResult<Result>> Update(string id, MailSave updateModel, [FromQuery] string changeToken = null) => UpdateModel<MailSave, MailEdit>(id, updateModel, changeToken);


  //[HttpDelete("{id}")]
  //[ZeroAuthorize(PagePermissions.Delete)]
  //public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}