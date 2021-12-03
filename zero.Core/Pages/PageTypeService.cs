using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Pages;

public class PageTypeService : IPageTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected FlavorOptions Flavors { get; set; }


  public PageTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    Flavors = options.For<FlavorOptions>();
  }


  /// <inheritdoc />
  public IEnumerable<FlavorConfig> GetAll()
  {
    return Flavors.GetAll<Page>();
  }


  /// <inheritdoc />
  public FlavorConfig GetByAlias(string flavorAlias)
  {
    return Flavors.Get<Page>(flavorAlias);
  }


  /// <inheritdoc />
  public async Task<IEnumerable<FlavorConfig>> GetAllowedTypes(string pageParentId = null)
  {
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
      return Flavors.GetAll<Page>();
    }

    return (await handler.GetAllowedPageTypes(Context.Application, Flavors.GetAll<Page>(), parents))?.ToList() ?? new();
  }
}


public interface IPageTypeService
{
  /// <summary>
  /// Get all available page types
  /// </summary>
  IEnumerable<FlavorConfig> GetAll();

  /// <summary>
  /// Get all page types which are allowed below a selected parent page
  /// </summary>
  Task<IEnumerable<FlavorConfig>> GetAllowedTypes(string pageParentId = null);

  /// <summary>
  /// Get a specific page type by alias
  /// </summary>
  FlavorConfig GetByAlias(string flavorAlias);
}
