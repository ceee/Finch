namespace zero.Pages;

public class PagesStore : TreeEntityStore<Page>, IPagesStore
{
  protected IPageTypeService PageTypes { get; private set; }


  public PagesStore(IStoreContext context, IPageTypeService pageTypes) : base(context)
  {
    PageTypes = pageTypes;
  }


  /// <inheritdoc />
  public override async Task<bool> IsAllowedAsChild(Page model, string parentId)
  {
    IList<PageType> pageTypes = await PageTypes.GetAllowedPageTypes(parentId);
    return pageTypes.Any(x => x.Alias == model.PageTypeAlias);
  }


  /// <inheritdoc />
  public async Task<Page> Empty(string pageType, string parentId = null)
  {
    PageType type = PageTypes.GetPageType(pageType);

    if (type == null)
    {
      return null;
    }

    Page model = await Empty();
    model.PageTypeAlias = type.Alias;
    model.ParentId = parentId;

    if (!await IsAllowedAsChild(model, parentId))
    {
      return null; // TODO we have no way to return an error here :-/
    }

    return model;
  }
}


public interface IPagesStore : ITreeEntityStore<Page>
{
  /// <summary>
  /// Get new instance of an entity for a specific page type
  /// </summary>
  Task<Page> Empty(string pageType, string parentId = null);
}