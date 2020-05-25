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
  public class PagesApi<T> : AppAwareBackofficeApi, IPagesApi<T> where T : IPage
  {
    protected IZeroOptions Options { get; private set; }

    public PagesApi(IZeroOptions options, IBackofficeStore store) : base(store)
    {
      Options = options;
    }


    /// <inheritdoc />
    public async Task<T> GetById(string id)
    {
      return await GetById<T>(id);
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

      Page page = await GetById<Page>(parentId);
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


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await Save(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      return await DeleteById<T>(id);
    }
  }


  public interface IPagesApi<T> where T : IPage
  {
    /// <summary>
    /// Get page by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get all available page types
    /// </summary>
    IList<PageType> GetPageTypes();

    /// <summary>
    /// Get all page types which are allowed below a selected parent page
    /// </summary>
    Task<IList<PageType>> GetAllowedPageTypes(string parentId = null);

    /// <summary>
    /// Creates or updates a page
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a page by Id
    /// </summary>
    Task<EntityResult<T>> Delete(string id);
  }
}
