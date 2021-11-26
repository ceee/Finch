using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Pages;

public class PageModuleTypeService : IPageModuleTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected IPagesStore PageStore { get; private set; }

  protected PageModuleOptions Options { get; set; }


  public PageModuleTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler, IPagesStore pageStore)
  {
    Context = context;
    Handler = handler;
    Options = options.For<PageModuleOptions>();
    PageStore = pageStore;
  }


  /// <inheritdoc />
  public async Task<IList<PageModuleType>> GetModuleTypes(string[] tags = default, string pageId = default)
  {
    List<PageModuleType> modules = Options;
    Page page = null;

    if (!pageId.IsNullOrEmpty())
    {
      page = await PageStore.Load(pageId);
    }

    if (tags?.Length > 0)
    {
      modules = Options.Where(x => x.Tags.Any(t => tags.Contains(t, StringComparer.InvariantCultureIgnoreCase))).ToList();
    }

    IModuleTypeHandler handler = Handler.Get<IModuleTypeHandler>();

    // if there is no registered handler we just allow all page types
    if (handler == null)
    {
      return modules;
    }

    return handler.GetAllowedModuleTypes(Context.Application, Options, page, tags)?.ToList() ?? new();
  }


  /// <inheritdoc />
  public PageModuleType GetModuleType(string alias)
  {
    return Options.FirstOrDefault(x => x.Alias == alias);
  }
}


public interface IPageModuleTypeService
{
  /// <summary>
  /// Get all available module types (can be limited to the passed tags)
  /// </summary>
  Task<IList<PageModuleType>> GetModuleTypes(string[] tags = default, string pageId = default);

  /// <summary>
  /// Get a specific module type by alias
  /// </summary>
  PageModuleType GetModuleType(string alias);
}
