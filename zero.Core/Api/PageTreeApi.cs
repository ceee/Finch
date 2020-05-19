using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class PageTreeApi : IPageTreeApi
  {
    protected IZeroOptions Options { get; private set; }

    protected IAppAwareBackofficeStore Backoffice { get; private set; }


    public PageTreeApi(IAppAwareBackofficeStore backoffice, IZeroOptions options)
    {
      Backoffice = backoffice;
      Options = options;
    }


    /// <inheritdoc />
    public async Task<IList<TreeItem>> GetChildren(string parentId = null)
    {
      IList<TreeItem> items = new List<TreeItem>();
      IReadOnlyCollection<PageType> pageTypes = Options.Pages.GetAllItems();

      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        IList<Page> pages = await session
          .Query<Page>()
          .ForApp(Backoffice.AppId)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .ToListAsync();

        foreach (Page page in pages)
        {
          PageType pageType = pageTypes.FirstOrDefault(x => x.Alias == page.PageTypeAlias);

          if (pageType == null)
          {
            continue;
            // TODO the page type does not exist anymore
          }

          items.Add(new TreeItem()
          {
            Id = page.Id,
            Name = page.Name,
            HasChildren = true,
            ParentId = page.ParentId,
            Sort = page.Sort,
            Icon = pageType.Icon,
            IsInactive = !page.IsActive
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
          Name = "@page.recyclebin.name",
          Icon = "fth-trash",
          HasChildren = false
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
    Task<IList<TreeItem>> GetChildren(string parentId = null);
  }
}
