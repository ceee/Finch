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

namespace zero.Core.Api
{
  public class PageTreeApi : AppAwareBackofficeApi, IPageTreeApi
  {
    public PageTreeApi(IBackofficeStore store) : base(store) { }


    /// <inheritdoc />
    public async Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null)
    {
      IList<TreeItem> items = new List<TreeItem>();
      IReadOnlyCollection<PageType> pageTypes = Backoffice.Options.Pages.GetAllItems();
      string[] openIds = new string[0] { };

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        IList<IPage> pages = await session
          .Query<IPage>()
          .Scope(Scope)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .ToListAsync();


        // get hierarchy so we know if we should set the page to open
        if (!activeId.IsNullOrEmpty())
        {
          Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
            .ProjectInto<Pages_ByHierarchy.Result>()
            .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
            .Scope(Scope)
            .FirstOrDefaultAsync(x => x.Id == activeId);

          if (result != null)
          {
            openIds = result.Path.Select(x => x.Id).ToArray(); // .Union(new string[1] { activeId })
          }
        }


        // get children for all pages
        string[] pageIds = pages.Select(x => x.Id).ToArray();

        IList<Pages_WithChildren.Result> children = await session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
          .ProjectInto<Pages_WithChildren.Result>()
          .Scope(Scope)
          .Where(x => x.Id.In(pageIds))
          .ToListAsync();


        // build tree
        foreach (IPage page in pages)
        {
          PageType pageType = pageTypes.FirstOrDefault(x => x.Alias == page.PageTypeAlias);

          if (pageType == null)
          {
            continue;
            // TODO the page type does not exist anymore
          }

          int childCount = children.Count(x => x.Id == page.Id);

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
            Modifier = !page.IsActive ? new TreeItemModifier()
            {
              Icon = "fth-minus-circle color-yellow",
              Name = "Inactive"
            } : null
          });
        }
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
    Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null);
  }
}
