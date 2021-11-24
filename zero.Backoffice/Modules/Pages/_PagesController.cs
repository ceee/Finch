using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Routing;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PagesController : BackofficeCollectionController<Page, IPageService>
  {
    IRoutes Routes;

    public PagesController(IPageService collection, IRoutes routes) : base(collection)
    {
      Routes = routes;
    }


    public override async Task<EditModel<Page>> GetById([FromQuery] string id, [FromQuery] string changeVector = null)
    {
      Page entity = changeVector.IsNullOrEmpty() ? await Collection.GetById(id) : await Collection.GetRevision(changeVector);

      return entity == null ? null : Edit<Page, PageEditModel<Page>>(new PageEditModel<Page>()
      {
        Entity = entity,
        PageType = Collection.GetPageType(entity.PageTypeAlias),
        Urls = new List<string>() { await Routes.GetUrl(entity) }
      });
    }


    public override async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      IReadOnlyCollection<PageType> pageTypes = Options.Pages.GetAllItems();
      Dictionary<string, Page> pages = await Collection.GetByIds(ids.ToArray());
      Dictionary<Page, Route> routes = await Routes.GetRoutes(pages.Where(x => x.Value != null).Select(x => x.Value).ToArray());

      return Previews(pages, item =>
      {
        routes.TryGetValue(item, out Route route);

        PreviewModel model = new()
        {
          Id = item.Id,
          Icon = pageTypes.FirstOrDefault(x => x.Alias == item.PageTypeAlias)?.Icon ?? "fth-folder",
          Name = item.Name,
          Text = route?.Url.Or("No URL found")
        };

        PreviewTransform?.Invoke(item, model);
        return model;
      });

    }


    public async Task<IList<TreeItem>> GetChildren([FromQuery] string parent = null, [FromQuery] string active = null, [FromQuery] string search = null)
    {
      return await Collection.GetChildren(parent, active, search);
    }


    public PageType GetPageType([FromQuery] string alias) => Collection.GetPageType(alias);


    public async Task<List<string>> GetUrls([FromQuery] string pageId)
    {
      string url = await Routes.GetUrl<Page>(pageId);
      return url.HasValue() ? new List<string>() { url } : new List<string>();
    }


    public async Task<IList<PageType>> GetAllowedPageTypes([FromQuery] string parent = null) => await Collection.GetAllowedPageTypes(parent);


    public async Task<EditModel<Page>> GetEmptyByType([FromQuery] string type, [FromQuery] string parent = null) => Edit(await Collection.GetEmpty(type, parent));


    [HttpPost]
    public async Task<EntityResult<IList<Page>>> SaveSorting([FromBody] string[] ids) => await Collection.SaveSorting(ids);


    [HttpPost]
    public async Task<EntityResult<Page>> Move([FromBody] ActionCopyModel model) => await Collection.Move(model.Id, model.DestinationId);


    [HttpPost]
    public async Task<EntityResult<Page>> Copy([FromBody] ActionCopyModel model) => await Collection.Copy(model.Id, model.DestinationId, model.IncludeDescendants);


    [HttpPost]
    public async Task<EntityResult<string[]>> Restore([FromBody] ActionCopyModel model) => await Collection.Restore(model.Id, model.IncludeDescendants);


    [HttpDelete]
    public async Task<EntityResult<string[]>> DeleteRecursive([FromQuery] string id) => await Collection.Delete(id, recursive: true, moveToRecycleBin: true);
  }
}
