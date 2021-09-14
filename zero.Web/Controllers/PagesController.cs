using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Routing;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PagesController : BackofficeController
  {
    IPagesApi Api;
    IZeroStore Store;

    public PagesController(IPagesApi api, IZeroStore store)
    {
      Api = api;
      Store = store;
    }


    public async Task<IList<PageType>> GetAllowedPageTypes([FromQuery] string parent = null) => await Api.GetAllowedPageTypes(parent);

 
    public PageType GetPageType([FromQuery] string alias) => Api.GetPageType(alias);

    public async Task<PageEditModel<Page>> GetById([FromQuery] string id)
    {
      Page entity = await Api.GetById(id);

      if (entity == null)
      {
        return null;
      }

      return Edit<Page, PageEditModel<Page>>(new PageEditModel<Page>()
      {
        Entity = entity,
        //Revisions = await RevisionsApi.GetPaged<Page>(id),
        PageType = Api.GetPageType(entity.PageTypeAlias)
      });
    }


    public async Task<EditModel<Page>> GetEmpty([FromQuery] string type, [FromQuery] string parent = null)
    {
      return Edit(await Api.GetEmpty(type, parent));
    }


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      IReadOnlyCollection<PageType> pageTypes = Options.Pages.GetAllItems();

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      Dictionary<string, Page> pages = await session.LoadAsync<Page>(ids);
      Dictionary<string, Route> routes = await session.LoadAsync<Route>(pages.Where(x => x.Value != null).Select(x => "routes." + x.Value.Hash));

      return Previews(pages, item =>
      {
        PageType pageType = pageTypes.FirstOrDefault(x => x.Alias == item.PageTypeAlias);
        Route route = null;

        if (!routes.TryGetValue("routes." + item.Hash, out route) || route == null)
        {
          route = new Route()
          {
            Url = "No URL found" // TODO translate
          };
        }

        return new PreviewModel()
        {
          Id = item.Id,
          Icon = pageType?.Icon ?? "fth-folder",
          Name = item.Name,
          Text = route.Url
        };
      });
    }


    //public async Task<ListResult<Revision>> GetRevisions([FromQuery] string id, [FromQuery] int page = 1) => await RevisionsApi.GetPaged<Page>(id, page); 
    // TODO this endpoint is available when pages controller moved to BackofficeCollectionController


    public async Task<EntityResult<Page>> Save([FromBody] Page model) => await Api.Save(model);


    [HttpPost]
    public async Task<EntityResult<IList<Page>>> SaveSorting([FromBody] string[] ids) => await Api.SaveSorting(ids);


    [HttpPost]
    public async Task<EntityResult<Page>> Move([FromBody] ActionCopyModel model) => await Api.Move(model.Id, model.DestinationId);


    [HttpPost]
    public async Task<EntityResult<Page>> Copy([FromBody] ActionCopyModel model) => await Api.Copy(model.Id, model.DestinationId, model.IncludeDescendants);


    [HttpPost]
    public async Task<EntityResult<string[]>> Restore([FromBody] ActionCopyModel model) => await Api.Restore(model.Id, model.IncludeDescendants);


    public async Task<EntityResult<string[]>> Delete([FromQuery] string id) => await Api.Delete(id, true);
  }
}
