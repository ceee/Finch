using FluentValidation;

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
    IEnumerable<FlavorConfig> pageTypes = await PageTypes.GetAllowedTypes(parentId);
    return pageTypes.Any(x => x.Alias == model.Flavor);
  }


  /// <inheritdoc />
  public async Task<Page> Empty(string pageTypeAlias, string parentId = null)
  {
    FlavorConfig type = PageTypes.GetByAlias(pageTypeAlias);

    if (type == null)
    {
      return null;
    }

    Page model = await base.Empty(pageTypeAlias);
    model.ParentId = parentId;

    if (!await IsAllowedAsChild(model, parentId))
    {
      return null; // TODO we have no way to return an error here :-/
    }

    return model;
  }


  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Page> validator)
  {
    validator.RuleFor(x => x.Name).NotEmpty();
    validator.RuleFor(x => x.Flavor).NotEmpty();
  }
}


public interface IPagesStore : ITreeEntityStore<Page>
{
  /// <summary>
  /// Get new instance of an entity for a specific page type
  /// </summary>
  Task<Page> Empty(string pageType, string parentId = null);
}