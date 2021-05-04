using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Routing;

namespace zero.Core.Api
{
  public class PageTreeApi : BackofficeApi, IPageTreeApi
  {
    protected IRoutes Routes { get; set; }

    public PageTreeApi(IBackofficeStore store, IRoutes routes) : base(store)
    {
      Routes = routes;
    }


    /// <inheritdoc />
    public async Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null, string search = null)
    {
      IList<TreeItem> items = new List<TreeItem>();
      IReadOnlyCollection<PageType> pageTypes = Backoffice.Options.Pages.GetAllItems();
      string[] openIds = new string[0] { };
      IList<Page> pages = null;
      IList<Pages_WithChildren.Result> children = null;
      bool isSearch = !search.IsNullOrWhiteSpace();

      using IAsyncDocumentSession session = Store.OpenAsyncSession();


      if (isSearch)
      {
        pages = await session
          .Query<Page>()
          .SearchIf(x => x.Name, search, "*")
          .OrderBy(x => x.Sort, OrderingType.Long)
          .ToListAsync();

        var urls = await Routes.GetUrls(pages.ToArray());
        
        foreach (Page page in pages)
        {
          if (urls.TryGetValue(page, out string url))
          {
            page.Url = url;
          }
        }
      }
      else
      {

        pages = await session
          .Query<Page>()
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderBy(x => x.Sort, OrderingType.Long)
          .ToListAsync();


        // get hierarchy so we know if we should set the page to open
        if (!activeId.IsNullOrEmpty())
        {
          Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
            .ProjectInto<Pages_ByHierarchy.Result>()
            .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
            .FirstOrDefaultAsync(x => x.Id == activeId);

          if (result != null)
          {
            openIds = result.Path.Select(x => x.Id).ToArray(); // .Union(new string[1] { activeId })
          }
        }


        // get children for all pages
        string[] pageIds = pages.Select(x => x.Id).ToArray();

        children = await session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
          .ProjectInto<Pages_WithChildren.Result>()
          .Where(x => x.Id.In(pageIds))
          .ToListAsync();
      }


      // function to get modifier icon
      TreeItemModifier GetModifier(Page page)
      {
        if (page.PublishDate > DateTimeOffset.Now || page.UnpublishDate > DateTimeOffset.Now)
        {
          return new TreeItemModifier("@page.schedule.scheduled", "fth-clock");
        }
        if (!page.IsActive)
        {
          return new TreeItemModifier("@ui.inactive", "fth-minus-circle color-red");
        }
        return null;
      }


      // build tree
      foreach (Page page in pages)
      {
        PageType pageType = pageTypes.FirstOrDefault(x => x.Alias == page.PageTypeAlias);

        if (pageType == null)
        {
          continue;
          // TODO the page type does not exist anymore
        }

        int childCount = isSearch ? 0 : children.Count(x => x.Id == page.Id);      

        items.Add(new TreeItem()
        {
          Id = page.Id,
          Name = page.Name,
          HasChildren = childCount > 0,
          ChildCount = childCount,
          ParentId = page.ParentId,
          Sort = page.Sort,
          Icon = pageType.Icon,
          IsOpen = openIds.Contains(page.Id),
          IsInactive = !page.IsActive,
          HasActions = true,
          Modifier = GetModifier(page),
          Description = isSearch ? page.Url : null
        });
      }

      if (parentId.IsNullOrEmpty())
      {
        items.Add(new TreeItem()
        {
          Id = "recyclebin",
          ParentId = null,
          Sort = 99999,
          Name = "@recyclebin.name",
          Icon = "fth-trash",
          HasChildren = false,
          HasActions = true
        });
      }

      return items;
    }
  }


  public interface IPageTreeApi
  {
    /// <summary>
    /// Get all children for the current parent page (or root if empty)
    /// </summary>
    Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null, string search = null);
  }
}
