using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Pages;

public class PageTypeService : IPageTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected PageOptions PageOptions { get; set; }


  public PageTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    PageOptions = options.For<PageOptions>();
  }


  /// <inheritdoc />
  public IList<PageType> GetPageTypes()
  {
    return PageOptions.GetAllItems().ToList();
  }


  /// <inheritdoc />
  public PageType GetPageType(string pageTypeAlias)
  {
    return PageOptions.GetAllItems().FirstOrDefault(x => x.Alias == pageTypeAlias);
  }


  /// <inheritdoc />
  public async Task<IList<PageType>> GetAllowedPageTypes(string pageParentId = null)
  {
    IEnumerable<PageType> types = PageOptions.GetAllItems();
    List<Page> parents = new();

    var session = Context.Store.Session();

    if (!pageParentId.IsNullOrEmpty())
    {
      Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
        .ProjectInto<Pages_ByHierarchy.Result>()
        .Include<Pages_ByHierarchy.Result, Page>(x => x.Id)
        .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
        .FirstOrDefaultAsync(x => x.Id == pageParentId);

      if (result != null)
      {
        List<string> ids = result.Path.Select(x => x.Id).ToList();
        ids.Add(result.Id);
        parents = (await session.LoadAsync<Page>(ids)).Select(x => x.Value).Reverse().ToList();
      }
    }

    IPageTypeHandler handler = Handler.Get<IPageTypeHandler>();

    // if there is no registered handler we just allow all page types
    if (handler == null)
    {
      return types.ToList();
    }

    return (await handler.GetAllowedPageTypes(Context.Application, types, parents))?.ToList() ?? new();
  }
}


public interface IPageTypeService
{
  /// <summary>
  /// Get all available page types
  /// </summary>
  IList<PageType> GetPageTypes();

  /// <summary>
  /// Get all page types which are allowed below a selected parent page
  /// </summary>
  Task<IList<PageType>> GetAllowedPageTypes(string pageParentId = null);

  /// <summary>
  /// Get a specific page type by alias
  /// </summary>
  PageType GetPageType(string pageTypeAlias);
}
