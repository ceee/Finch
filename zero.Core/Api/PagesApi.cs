using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class PagesApi : IPagesApi
  {
    protected IZeroOptions Options { get; private set; }

    protected IAppAwareBackofficeStore Backoffice { get; private set; }


    public PagesApi(IAppAwareBackofficeStore backoffice, IZeroOptions options)
    {
      Backoffice = backoffice;
      Options = options;
    }


    /// <inheritdoc />
    public async Task<IList<Page>> GetChildren(string parentId = null)
    {
      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        return await session
          .Query<Page>()
          .ForApp(Backoffice.AppId)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public IList<PageType> GetPageTypes()
    {
      return Options.Pages.GetAllItems().ToList();
    }


    /// <inheritdoc />
    public async Task<IList<PageType>> GetAllowedPageTypes(string parentId = null)
    {    
      IEnumerable<PageType> types = Options.Pages.GetAllItems();

      if (parentId.IsNullOrEmpty())
      {
        return types.Where(x => x.AllowAsRoot).ToList();
      }

      Page page = await Backoffice.GetById<Page>(parentId);
      PageType pageType = page != null ? types.FirstOrDefault(x => x.Alias == page.PageTypeAlias) : null;

      if (pageType == null)
      {
        return new List<PageType>();
      }

      if (pageType.AllowAllChildrenTypes)
      {
        return types.ToList();
      }

      return types.Where(x => pageType.AllowedChildrenTypes.Contains(x.Alias)).ToList();
    }
  }


  public interface IPagesApi
  {
    /// <summary>
    /// Get all available page types
    /// </summary>
    IList<PageType> GetPageTypes();

    /// <summary>
    /// Get all page types which are allowed below a selected parent page
    /// </summary>
    Task<IList<PageType>> GetAllowedPageTypes(string parentId = null);
  }
}
