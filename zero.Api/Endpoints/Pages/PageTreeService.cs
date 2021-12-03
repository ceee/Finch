using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace zero.Api.Endpoints.Pages;

[Obsolete("Move to zero.Backoffice")]
public class PageTreeService : IPageTreeService
{
  protected IPagesStore Pages { get; private set; }

  protected IRoutes Routes { get; set; }

  protected IPageTypeService PageTypes { get; set; }


  public PageTreeService(IPagesStore pages, IPageTypeService pageTypes, IRoutes routes)
  {
    Pages = pages;
    Routes = routes;
    PageTypes = pageTypes;
  }


  /// <inheritdoc />
  public async Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null, string search = null)
  {
    List<TreeItem> items = new();
    string[] openIds = Array.Empty<string>();
    Paged<Page> pages = null;
    IList<Pages_WithChildren.Result> children = null;
    bool isSearch = !search.IsNullOrWhiteSpace();

    if (isSearch)
    {
      pages = await Pages.Load(1, Int32.MaxValue, q => q.SearchIf(x => x.Name, search, "*").OrderBy(x => x.Sort, OrderingType.Long));

      var urls = await Routes.GetUrls(pages.Items.ToArray());

      foreach (Page page in pages.Items)
      {
        if (urls.TryGetValue(page, out string url))
        {
          page.Url = url;
        }
      }
    }
    else
    {
      pages = await Pages.Load(1, Int32.MaxValue, q => q
        .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
        .OrderBy(x => x.Sort, OrderingType.Long));


      // get hierarchy so we know if we should set the page to open
      if (!activeId.IsNullOrEmpty())
      {
        Pages_ByHierarchy.Result result = await Pages.Session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
          .ProjectInto<Pages_ByHierarchy.Result>()
          .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
          .FirstOrDefaultAsync(x => x.Id == activeId);

        if (result != null)
        {
          openIds = result.Path.Select(x => x.Id).ToArray(); // .Union(new string[1] { activeId })
        }
      }


      // get children for all pages
      string[] pageIds = pages.Items.Select(x => x.Id).ToArray();

      children = await Pages.Session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
        .ProjectInto<Pages_WithChildren.Result>()
        .Where(x => x.Id.In(pageIds))
        .ToListAsync();
    }


    // function to get modifier icon
    TreeItemModifier? GetModifier(Page page)
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
    foreach (Page page in pages.Items)
    {
      FlavorConfig pageType = PageTypes.GetByAlias(page.Flavor);

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


public interface IPageTreeService
{
  /// <summary>
  /// Get page children as tree items
  /// </summary>
  Task<IList<TreeItem>> GetChildren(string parentId = null, string activeId = null, string search = null);
}